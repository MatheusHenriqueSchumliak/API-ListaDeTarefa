using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ListaDeTarefa.Infrastructure.Events
{
    public class ProcessadorEventosService(FilaEventos fila, IServiceScopeFactory scopeFactory, ILogger<ProcessadorEventosService> logger) : BackgroundService
    {
        #region Construtor
        private readonly FilaEventos _fila = fila;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly ILogger<ProcessadorEventosService> _logger = logger;
        #endregion

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await foreach (var evento in _fila.LerTodosAsync(stoppingToken))
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var despachante = scope.ServiceProvider.GetRequiredService<DespachanteEventos>();
                        await despachante.DespacharAsync([evento], stoppingToken);
                    }
                    catch (Exception ex) when (ex is not OperationCanceledException)
                    {
                        _logger.LogError(ex, "Falha ao processar o evento {Evento} da fila.", evento.GetType().Name);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Processador de eventos finalizado.");
            }
        }
    }
}
