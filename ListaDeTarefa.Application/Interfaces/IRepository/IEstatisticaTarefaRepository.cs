using ListaDeTarefa.Domain.Entities;

namespace ListaDeTarefa.Application.Interfaces.IRepository
{
    public interface IEstatisticaTarefaRepository : IGenericRepository<EstatisticaTarefa>
    {
        Task<EstatisticaTarefa?> BuscarPorTarefaId(Guid tarefaId);
    }
}
