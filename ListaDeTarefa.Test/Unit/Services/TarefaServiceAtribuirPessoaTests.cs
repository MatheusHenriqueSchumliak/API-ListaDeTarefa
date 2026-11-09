using ListaDeTarefa.Application.Interfaces;
using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Application.Services;
using ListaDeTarefa.Domain.Entities;
using ListaDeTarefa.Domain.Events;
using Moq;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Services
{
    public class TarefaServiceAtribuirPessoaTests
    {
        private readonly Mock<ITarefaRepository> _tarefaRepo = new();
        private readonly Mock<IPessoaRepository> _pessoaRepo = new();
        private readonly Mock<IDespachanteEventos> _despachante = new();
        private readonly TarefaService _service;

        public TarefaServiceAtribuirPessoaTests()
        {
            _service = new TarefaService(_tarefaRepo.Object, _pessoaRepo.Object, _despachante.Object);
        }

        private static Pessoa CriarPessoa()
            => Pessoa.Criar("Ana", "Silva", "ana@email.com", "11999999999", "11999999999", DateTimeOffset.UtcNow.AddYears(-30), default);

        [Fact]
        public async Task AtribuirPessoa_DeveDespacharEventosSomenteDepoisDePersistir()
        {
            // Arrange
            var tarefa = Tarefa.Criar("Lavar o carro");
            var pessoa = CriarPessoa();
            _tarefaRepo.Setup(r => r.BuscarPorId(tarefa.Id)).ReturnsAsync(tarefa);
            _pessoaRepo.Setup(r => r.BuscarPorId(pessoa.Id)).ReturnsAsync(pessoa);

            var ordem = new List<string>();
            IReadOnlyList<IEventoDominio>? despachados = null;
            _tarefaRepo.Setup(r => r.Atualizar(tarefa)).Callback(() => ordem.Add("persistiu")).Returns(Task.CompletedTask);
            _despachante.Setup(d => d.DespacharAsync(It.IsAny<IEnumerable<IEventoDominio>>(), It.IsAny<CancellationToken>()))
                .Callback<IEnumerable<IEventoDominio>, CancellationToken>((eventos, _) =>
                {
                    ordem.Add("despachou");
                    despachados = eventos.ToList();
                })
                .Returns(Task.CompletedTask);

            // Act
            await _service.AtribuirPessoa(tarefa.Id, pessoa.Id);

            // Assert
            Assert.Equal(["persistiu", "despachou"], ordem);
            var evento = Assert.IsType<TarefaAtribuidaEvento>(Assert.Single(despachados!));
            Assert.Equal(pessoa.Id, evento.PessoaId);
            Assert.Empty(tarefa.Eventos);
        }

        [Fact]
        public async Task AtribuirPessoa_NaoDeveDespachar_QuandoPersistenciaFalha()
        {
            // Arrange
            var tarefa = Tarefa.Criar("Lavar o carro");
            var pessoa = CriarPessoa();
            _tarefaRepo.Setup(r => r.BuscarPorId(tarefa.Id)).ReturnsAsync(tarefa);
            _pessoaRepo.Setup(r => r.BuscarPorId(pessoa.Id)).ReturnsAsync(pessoa);
            _tarefaRepo.Setup(r => r.Atualizar(tarefa)).ThrowsAsync(new InvalidOperationException("falha no banco"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AtribuirPessoa(tarefa.Id, pessoa.Id));
            _despachante.Verify(d => d.DespacharAsync(It.IsAny<IEnumerable<IEventoDominio>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AtribuirPessoa_DeveLancarExcecao_QuandoTarefaNaoExiste()
        {
            _tarefaRepo.Setup(r => r.BuscarPorId(It.IsAny<Guid>())).ReturnsAsync((Tarefa?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AtribuirPessoa(Guid.NewGuid(), Guid.NewGuid()));

            _despachante.Verify(d => d.DespacharAsync(It.IsAny<IEnumerable<IEventoDominio>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AtribuirPessoa_DeveLancarExcecao_QuandoPessoaNaoExiste()
        {
            var tarefa = Tarefa.Criar("Lavar o carro");
            _tarefaRepo.Setup(r => r.BuscarPorId(tarefa.Id)).ReturnsAsync(tarefa);
            _pessoaRepo.Setup(r => r.BuscarPorId(It.IsAny<Guid>())).ReturnsAsync((Pessoa?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AtribuirPessoa(tarefa.Id, Guid.NewGuid()));

            _tarefaRepo.Verify(r => r.Atualizar(It.IsAny<Tarefa>()), Times.Never);
            _despachante.Verify(d => d.DespacharAsync(It.IsAny<IEnumerable<IEventoDominio>>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
