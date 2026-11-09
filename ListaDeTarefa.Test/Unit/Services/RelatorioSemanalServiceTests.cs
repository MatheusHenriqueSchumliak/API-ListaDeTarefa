using ListaDeTarefa.Application.DTOs.RelatorioSemanal;
using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Application.Services;
using ListaDeTarefa.Domain.Entities;
using ListaDeTarefa.Test.Fixtures;
using Moq;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Services
{
    public class RelatorioSemanalServiceTests
    {
        private static readonly DateTimeOffset _referencia = DateTimeOffset.Parse("2025-06-10T10:00:00-03:00");
        private static readonly DateTimeOffset _inicioEsperado = DateTimeOffset.Parse("2025-06-02T03:00:00Z");
        private static readonly DateTimeOffset _fimEsperado = DateTimeOffset.Parse("2025-06-09T03:00:00Z");

        private readonly Mock<ITarefaRepository> _tarefaRepo = new();
        private readonly Mock<IRelatorioSemanalRepository> _relatorioRepo = new();
        private readonly RelatorioSemanalService _service;

        public RelatorioSemanalServiceTests()
        {
            var calculadora = new CalculadoraPeriodoSemana(new TimeProviderFalso(_referencia, TimeProviderFalso.SaoPaulo));
            _service = new RelatorioSemanalService(_tarefaRepo.Object, _relatorioRepo.Object, calculadora);
        }

        [Fact]
        public async Task GerarRelatorioSemanal_DeveCriarERetornarRelatorio_QuandoNaoExiste()
        {
            // Arrange
            _relatorioRepo.Setup(r => r.BuscarPorInicioSemana(_inicioEsperado)).ReturnsAsync((RelatorioSemanal?)null);
            _tarefaRepo.Setup(r => r.ObterResumoPorPeriodo(_inicioEsperado, _fimEsperado))
                .ReturnsAsync(new ResumoTarefasPeriodoDto(5, 3, 1, 12.5m));
            RelatorioSemanal? salvo = null;
            _relatorioRepo.Setup(r => r.Adicionar(It.IsAny<RelatorioSemanal>()))
                .Callback<RelatorioSemanal>(r => salvo = r)
                .Returns(Task.CompletedTask);

            // Act
            var resultado = await _service.GerarRelatorioSemanal(_referencia);

            // Assert
            _relatorioRepo.Verify(r => r.Adicionar(It.IsAny<RelatorioSemanal>()), Times.Once);
            Assert.NotNull(salvo);
            Assert.Equal(_inicioEsperado, salvo!.InicioSemana);
            Assert.Equal(_fimEsperado, salvo.FimSemana);
            Assert.Equal(5, resultado.TotalCriadas);
            Assert.Equal(3, resultado.TotalConcluidas);
            Assert.Equal(1, resultado.TotalPendentes);
            Assert.Equal(75m, resultado.TaxaConclusao);
            Assert.Equal(12.5m, resultado.TempoMedioConclusaoHoras);
        }

        [Fact]
        public async Task GerarRelatorioSemanal_NaoDeveDuplicar_QuandoJaExisteRelatorioDaSemana()
        {
            // Arrange
            var existente = RelatorioSemanal.Criar(_inicioEsperado, _fimEsperado, 1, 1, 0, 100m, 2m);
            _relatorioRepo.Setup(r => r.BuscarPorInicioSemana(_inicioEsperado)).ReturnsAsync(existente);

            // Act
            var resultado = await _service.GerarRelatorioSemanal(_referencia);

            // Assert
            Assert.Equal(existente.Id, resultado.Id);
            _relatorioRepo.Verify(r => r.Adicionar(It.IsAny<RelatorioSemanal>()), Times.Never);
            _tarefaRepo.Verify(r => r.ObterResumoPorPeriodo(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()), Times.Never);
        }

        [Fact]
        public async Task GerarRelatorioSemanal_DeveTerTaxaZero_QuandoNaoHaTarefas()
        {
            // Arrange
            _relatorioRepo.Setup(r => r.BuscarPorInicioSemana(It.IsAny<DateTimeOffset>())).ReturnsAsync((RelatorioSemanal?)null);
            _tarefaRepo.Setup(r => r.ObterResumoPorPeriodo(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(new ResumoTarefasPeriodoDto(0, 0, 0, 0m));
            _relatorioRepo.Setup(r => r.Adicionar(It.IsAny<RelatorioSemanal>())).Returns(Task.CompletedTask);

            // Act
            var resultado = await _service.GerarRelatorioSemanal(_referencia);

            // Assert
            Assert.Equal(0m, resultado.TaxaConclusao);
        }

        [Fact]
        public async Task GerarRelatorioSemanal_NaoDevePassarDeCemPorCento_QuandoConcluiuTarefasDeSemanasAnteriores()
        {
            // Arrange: criadas = 1, mas concluídas = 4 (tarefas antigas concluídas nesta semana)
            _relatorioRepo.Setup(r => r.BuscarPorInicioSemana(It.IsAny<DateTimeOffset>())).ReturnsAsync((RelatorioSemanal?)null);
            _tarefaRepo.Setup(r => r.ObterResumoPorPeriodo(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
                .ReturnsAsync(new ResumoTarefasPeriodoDto(1, 4, 0, 30m));
            _relatorioRepo.Setup(r => r.Adicionar(It.IsAny<RelatorioSemanal>())).Returns(Task.CompletedTask);

            // Act
            var resultado = await _service.GerarRelatorioSemanal(_referencia);

            // Assert
            Assert.Equal(100m, resultado.TaxaConclusao);
        }

        [Fact]
        public async Task BuscarUltimo_DeveRetornarNulo_QuandoNaoExisteRelatorio()
        {
            _relatorioRepo.Setup(r => r.BuscarUltimo()).ReturnsAsync((RelatorioSemanal?)null);

            var resultado = await _service.BuscarUltimo();

            Assert.Null(resultado);
        }
    }
}
