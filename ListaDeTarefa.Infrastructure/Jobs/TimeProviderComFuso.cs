namespace ListaDeTarefa.Infrastructure.Jobs
{
    public class TimeProviderComFuso(TimeZoneInfo fuso) : TimeProvider
    {
        private readonly TimeZoneInfo _fuso = fuso;

        public override TimeZoneInfo LocalTimeZone => _fuso;
    }
}
