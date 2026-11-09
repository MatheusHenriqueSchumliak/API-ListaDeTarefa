using ListaDeTarefa.Application.DTOs.EstatisticaTarefa;

namespace ListaDeTarefa.Application.Interfaces.IService
{
    public interface IEstatisticaTarefaService
    {
        Task<EstatisticaTarefaDto?> BuscarPorTarefaId(Guid tarefaId);
    }
}
