using ListaDeTarefa.Application.Interfaces;
using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Application.Manipuladores;
using ListaDeTarefa.Domain.Entities;
using ListaDeTarefa.Domain.Events;
using Moq;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Manipuladores
{
    public class TarefaAtribuidaManipuladorTests
    {
        private readonly Mock<IPessoaRepository> _pessoaRepo = new();
        private readonly Mock<INotificador> _notificador = new();
        private readonly TarefaAtribuidaManipulador _manipulador;

        public TarefaAtribuidaManipuladorTests()
        {
            _manipulador = new TarefaAtribuidaManipulador(_pessoaRepo.Object, _notificador.Object);
        }

        [Fact]
        public async Task ManipularAsync_DeveNotificarPessoaComMensagemCorreta()
        {
            // Arrange
            var pessoa = Pessoa.Criar("Ana", "Silva", "ana@email.com", "11999999999", "11999999999", DateTimeOffset.UtcNow.AddYears(-30), default);
            _pessoaRepo.Setup(r => r.BuscarPorId(pessoa.Id)).ReturnsAsync(pessoa);
            var evento = new TarefaAtribuidaEvento(Guid.NewGuid(), pessoa.Id, "Lavar o carro", DateTimeOffset.UtcNow);

            // Act
            await _manipulador.ManipularAsync(evento);

            // Assert
            _notificador.Verify(n => n.EnviarAsync(
                pessoa,
                "Olá Ana, a tarefa 'Lavar o carro' foi atribuída a você",
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ManipularAsync_NaoDeveNotificar_QuandoPessoaNaoExiste()
        {
            // Arrange
            _pessoaRepo.Setup(r => r.BuscarPorId(It.IsAny<Guid>())).ReturnsAsync((Pessoa?)null);
            var evento = new TarefaAtribuidaEvento(Guid.NewGuid(), Guid.NewGuid(), "Qualquer", DateTimeOffset.UtcNow);

            // Act
            await _manipulador.ManipularAsync(evento);

            // Assert
            _notificador.Verify(n => n.EnviarAsync(It.IsAny<Pessoa>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
