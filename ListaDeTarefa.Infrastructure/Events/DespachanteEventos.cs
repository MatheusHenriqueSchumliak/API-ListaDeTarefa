using ListaDeTarefa.Application.Interfaces;
using ListaDeTarefa.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ListaDeTarefa.Infrastructure.Events
{
    public class DespachanteEventos(IServiceProvider serviceProvider, ILogger<DespachanteEventos> logger) : IDespachanteEventos
    {
        #region Construtor
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<DespachanteEventos> _logger = logger;
        #endregion

        public async Task DespacharAsync(IEnumerable<IEventoDominio> eventos, CancellationToken cancellationToken = default)
        {
            foreach (var evento in eventos.ToList())
            {
                var tipoEvento = evento.GetType();
                var tipoManipulador = typeof(IManipuladorEvento<>).MakeGenericType(tipoEvento);
                var metodo = tipoManipulador.GetMethod(nameof(IManipuladorEvento<IEventoDominio>.ManipularAsync))!;

                foreach (var manipulador in _serviceProvider.GetServices(tipoManipulador))
                {
                    try
                    {
                        await (Task)metodo.Invoke(manipulador, [evento, cancellationToken])!;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Falha no manipulador {Manipulador} ao tratar o evento {Evento}.",
                            manipulador!.GetType().Name, tipoEvento.Name);
                    }
                }
            }
        }
    }
}
