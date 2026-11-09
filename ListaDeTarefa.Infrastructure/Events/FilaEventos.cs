using System.Threading.Channels;
using ListaDeTarefa.Domain.Events;

namespace ListaDeTarefa.Infrastructure.Events
{
    public class FilaEventos
    {
        private readonly Channel<IEventoDominio> _canal = Channel.CreateBounded<IEventoDominio>(
            new BoundedChannelOptions(1000)
            {
                SingleReader = true,
                FullMode = BoundedChannelFullMode.Wait
            });

        public ValueTask EnfileirarAsync(IEventoDominio evento, CancellationToken cancellationToken = default)
            => _canal.Writer.WriteAsync(evento, cancellationToken);

        public IAsyncEnumerable<IEventoDominio> LerTodosAsync(CancellationToken cancellationToken)
            => _canal.Reader.ReadAllAsync(cancellationToken);
    }
}
