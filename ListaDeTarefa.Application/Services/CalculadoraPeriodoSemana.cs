using ListaDeTarefa.Application.DTOs.RelatorioSemanal;

namespace ListaDeTarefa.Application.Services
{
    public class CalculadoraPeriodoSemana(TimeProvider timeProvider)
    {
        #region Construtor
        private readonly TimeProvider _timeProvider = timeProvider;
        #endregion Construtor

        public PeriodoSemanaDto CalcularSemanaAnterior()
        {
            return CalcularSemanaAnterior(_timeProvider.GetUtcNow());
        }

        public PeriodoSemanaDto CalcularSemanaAnterior(DateTimeOffset referencia)
        {
            var fuso = _timeProvider.LocalTimeZone;
            var referenciaLocal = TimeZoneInfo.ConvertTime(referencia, fuso);

            var diasDesdeSegunda = ((int)referenciaLocal.DayOfWeek + 6) % 7;
            var segundaAtualLocal = referenciaLocal.Date.AddDays(-diasDesdeSegunda);
            var segundaAnteriorLocal = segundaAtualLocal.AddDays(-7);

            return new PeriodoSemanaDto(ParaUtc(segundaAnteriorLocal, fuso), ParaUtc(segundaAtualLocal, fuso));
        }

        private static DateTimeOffset ParaUtc(DateTime dataLocal, TimeZoneInfo fuso)
        {
            var naoEspecificada = DateTime.SpecifyKind(dataLocal, DateTimeKind.Unspecified);
            var utc = TimeZoneInfo.ConvertTimeToUtc(naoEspecificada, fuso);
            return new DateTimeOffset(utc, TimeSpan.Zero);
        }
    }
}
