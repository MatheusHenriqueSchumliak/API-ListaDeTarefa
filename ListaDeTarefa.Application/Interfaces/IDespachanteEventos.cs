using ListaDeTarefa.Domain.Events;

namespace ListaDeTarefa.Application.Interfaces
{
    public interface IDespachanteEventos
    {
        Task DespacharAsync(IEnumerable<IEventoDominio> eventos, CancellationToken cancellationToken = default);
    }
}
