using ListaDeTarefa.Application.Interfaces;
using ListaDeTarefa.Domain.Events;

namespace ListaDeTarefa.Infrastructure.Events
{
    public class DespachanteEventosEmFila(FilaEventos fila) : IDespachanteEventos
    {
        #region Construtor
        private readonly FilaEventos _fila = fila;
        #endregion

        public async Task DespacharAsync(IEnumerable<IEventoDominio> eventos, CancellationToken cancellationToken = default)
        {
            foreach (var evento in eventos)
                await _fila.EnfileirarAsync(evento, cancellationToken);
        }
    }
}
