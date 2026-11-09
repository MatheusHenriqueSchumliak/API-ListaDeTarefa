using ListaDeTarefa.Domain.Events;

namespace ListaDeTarefa.Application.Interfaces
{
    public interface IManipuladorEvento<in TEvento> where TEvento : IEventoDominio
    {
        Task ManipularAsync(TEvento evento, CancellationToken cancellationToken = default);
    }
}
