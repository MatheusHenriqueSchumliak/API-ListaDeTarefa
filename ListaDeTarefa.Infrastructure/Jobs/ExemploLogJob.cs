using Microsoft.Extensions.Logging;

namespace ListaDeTarefa.Infrastructure.Jobs
{
    public class ExemploLogJob(ILogger<ExemploLogJob> logger) : BaseJob(logger)
    {
        public override string Nome => "ExemploLogJob";

        protected override Task ExecutarJobAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Olá! Sou um job de exemplo, executado em {DataHoraUtc}.", DateTimeOffset.UtcNow);
            return Task.CompletedTask;
        }
    }
}
