using ListaDeTarefa.Domain.Entities;

namespace ListaDeTarefa.Application.Interfaces
{
    public interface INotificador
    {
        Task EnviarAsync(Pessoa pessoa, string mensagem, CancellationToken cancellationToken = default);
    }
}
