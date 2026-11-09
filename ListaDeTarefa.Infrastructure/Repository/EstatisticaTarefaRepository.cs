using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Infrastructure.Context;
using ListaDeTarefa.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ListaDeTarefa.Infrastructure.Repository
{
    public class EstatisticaTarefaRepository : GenericRepository<EstatisticaTarefa>, IEstatisticaTarefaRepository
    {
        #region Construtor
        public EstatisticaTarefaRepository(ListaDeTarefaContexto context) : base(context) { }
        #endregion Construtor

        public async Task<EstatisticaTarefa?> BuscarPorTarefaId(Guid tarefaId)
        {
            try
            {
                return await _dbSet.FirstOrDefaultAsync(e => e.TarefaId == tarefaId);
            }
            catch (Exception ex)
            {
                throw new Exception(" ", ex);
            }
        }
    }
}
