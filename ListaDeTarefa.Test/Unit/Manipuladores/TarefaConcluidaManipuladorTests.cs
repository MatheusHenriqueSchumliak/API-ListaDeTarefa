using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Application.Manipuladores;
using ListaDeTarefa.Domain.Entities;
using ListaDeTarefa.Domain.Events;
using Moq;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Manipuladores
{
    public class TarefaConcluidaManipuladorTests
    {
        private readonly Mock<IEstatisticaTarefaRepository> _repositorio = new();
        private readonly TarefaConcluidaManipulador _manipulador;

        public TarefaConcluidaManipuladorTests()
        {
            _manipulador = new TarefaConcluidaManipulador(_repositorio.Object);
        }

        [Fact]
        public async Task ManipularAsync_DeveGravarEstatisticaComDuracaoCorreta()
        {
            // Arrange
            var criacao = new DateTimeOffset(2025, 6, 2, 10, 0, 0, TimeSpan.Zero);
            var evento = new TarefaConcluidaEvento(Guid.NewGuid(), Guid.NewGuid(), criacao, criacao.AddMinutes(45));
            _repositorio.Setup(r => r.BuscarPorTarefaId(evento.TarefaId)).ReturnsAsync((EstatisticaTarefa?)null);

            // Act
            await _manipulador.ManipularAsync(evento);

            // Assert
            _repositorio.Verify(r => r.Adicionar(It.Is<EstatisticaTarefa>(e =>
                e.TarefaId == evento.TarefaId &&
                e.PessoaId == evento.PessoaId &&
                e.DuracaoEmMinutos == 45)), Times.Once);
        }

        [Fact]
        public async Task ManipularAsync_NaoDeveDuplicar_QuandoJaExisteEstatisticaDaTarefa()
        {
            // Arrange
            var criacao = DateTimeOffset.UtcNow.AddMinutes(-10);
            var evento = new TarefaConcluidaEvento(Guid.NewGuid(), null, criacao, DateTimeOffset.UtcNow);
            var existente = EstatisticaTarefa.Criar(evento.TarefaId, null, criacao, evento.DataConclusao);
            _repositorio.Setup(r => r.BuscarPorTarefaId(evento.TarefaId)).ReturnsAsync(existente);

            // Act
            await _manipulador.ManipularAsync(evento);

            // Assert
            _repositorio.Verify(r => r.Adicionar(It.IsAny<EstatisticaTarefa>()), Times.Never);
        }
    }
}
