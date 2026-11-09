using ListaDeTarefa.Application.DTOs.EstatisticaTarefa;
using ListaDeTarefa.Application.Factories;
using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Application.Interfaces.IService;

namespace ListaDeTarefa.Application.Services
{
    public class EstatisticaTarefaService(IEstatisticaTarefaRepository estatisticaRepository) : IEstatisticaTarefaService
    {
        #region Construtor
        private readonly IEstatisticaTarefaRepository _estatisticaRepository = estatisticaRepository;
        #endregion Construtor

        public async Task<EstatisticaTarefaDto?> BuscarPorTarefaId(Guid tarefaId)
        {
            var estatistica = await _estatisticaRepository.BuscarPorTarefaId(tarefaId);
            return estatistica is null ? null : EstatisticaTarefaFactory.EntidadeParaDto(estatistica);
        }
    }
}
