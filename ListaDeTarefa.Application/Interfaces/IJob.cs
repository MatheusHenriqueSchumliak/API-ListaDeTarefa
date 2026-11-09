namespace ListaDeTarefa.Application.Interfaces
{
    public interface IJob
    {
        string Nome { get; }
        Task ExecutarAsync(CancellationToken cancellationToken);
    }
}
