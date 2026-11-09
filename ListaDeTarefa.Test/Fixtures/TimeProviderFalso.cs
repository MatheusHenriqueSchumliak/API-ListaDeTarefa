namespace ListaDeTarefa.Test.Fixtures
{
    public class TimeProviderFalso(DateTimeOffset agora, TimeZoneInfo? fuso = null) : TimeProvider
    {
        public static TimeZoneInfo SaoPaulo => TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

        public override DateTimeOffset GetUtcNow() => agora.ToUniversalTime();

        public override TimeZoneInfo LocalTimeZone => fuso ?? TimeZoneInfo.Utc;
    }
}
