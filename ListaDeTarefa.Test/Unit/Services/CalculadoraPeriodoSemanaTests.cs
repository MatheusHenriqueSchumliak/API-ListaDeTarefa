using ListaDeTarefa.Application.Services;
using ListaDeTarefa.Test.Fixtures;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Services
{
    public class CalculadoraPeriodoSemanaTests
    {
        private static CalculadoraPeriodoSemana CriarCalculadora(DateTimeOffset agora)
            => new(new TimeProviderFalso(agora, TimeProviderFalso.SaoPaulo));

        [Theory]
        [InlineData("2025-06-10T10:00:00-03:00")] // terça
        [InlineData("2025-06-09T08:00:00-03:00")] // segunda (hora do job)
        [InlineData("2025-06-09T00:00:00-03:00")] // segunda à meia-noite exata
        [InlineData("2025-06-15T23:59:00-03:00")] // domingo à noite
        public void CalcularSemanaAnterior_DeveRetornarSegundaASegundaAnterior(string referencia)
        {
            var calculadora = CriarCalculadora(DateTimeOffset.Parse(referencia));

            var periodo = calculadora.CalcularSemanaAnterior(DateTimeOffset.Parse(referencia));

            Assert.Equal(DateTimeOffset.Parse("2025-06-02T00:00:00-03:00"), periodo.Inicio);
            Assert.Equal(DateTimeOffset.Parse("2025-06-09T00:00:00-03:00"), periodo.Fim);
        }

        [Fact]
        public void CalcularSemanaAnterior_DeveRetornarDatasComOffsetZero()
        {
            var calculadora = CriarCalculadora(DateTimeOffset.Parse("2025-06-10T10:00:00-03:00"));

            var periodo = calculadora.CalcularSemanaAnterior();

            Assert.Equal(TimeSpan.Zero, periodo.Inicio.Offset);
            Assert.Equal(TimeSpan.Zero, periodo.Fim.Offset);
        }

        [Fact]
        public void CalcularSemanaAnterior_SemParametro_DeveUsarRelogioInjetado()
        {
            var calculadora = CriarCalculadora(DateTimeOffset.Parse("2025-06-10T10:00:00-03:00"));

            var periodo = calculadora.CalcularSemanaAnterior();

            Assert.Equal(DateTimeOffset.Parse("2025-06-02T03:00:00Z"), periodo.Inicio);
            Assert.Equal(DateTimeOffset.Parse("2025-06-09T03:00:00Z"), periodo.Fim);
        }

        [Fact]
        public void CalcularSemanaAnterior_DeveTerSempreSeteDiasDeDuracaoEmFusoSemHorarioDeVerao()
        {
            var calculadora = CriarCalculadora(DateTimeOffset.Parse("2025-06-10T10:00:00-03:00"));

            var periodo = calculadora.CalcularSemanaAnterior();

            Assert.Equal(TimeSpan.FromDays(7), periodo.Fim - periodo.Inicio);
        }
    }
}
