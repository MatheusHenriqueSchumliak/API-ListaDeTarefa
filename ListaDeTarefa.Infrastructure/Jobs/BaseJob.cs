using ListaDeTarefa.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ListaDeTarefa.Infrastructure.Jobs
{
    public abstract class BaseJob(ILogger logger) : IJob
    {
        #region Construtor
        protected readonly ILogger _logger = logger;
        #endregion Construtor

        public abstract string Nome { get; }

        protected abstract Task ExecutarJobAsync(CancellationToken cancellationToken);

        public async Task ExecutarAsync(CancellationToken cancellationToken)
        {
            var cronometro = Stopwatch.StartNew();
            _logger.LogInformation("Job {Nome} iniciado.", Nome);

            try
            {
                await ExecutarJobAsync(cancellationToken);
                _logger.LogInformation("Job {Nome} finalizado com sucesso em {Duracao} ms.", Nome, cronometro.ElapsedMilliseconds);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning("Job {Nome} cancelado após {Duracao} ms.", Nome, cronometro.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Job {Nome} falhou após {Duracao} ms.", Nome, cronometro.ElapsedMilliseconds);
            }
        }
    }
}
