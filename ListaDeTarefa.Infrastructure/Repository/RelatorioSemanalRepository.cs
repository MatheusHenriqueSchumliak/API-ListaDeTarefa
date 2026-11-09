using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Infrastructure.Context;
using ListaDeTarefa.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ListaDeTarefa.Infrastructure.Repository
{
    public class RelatorioSemanalRepository : GenericRepository<RelatorioSemanal>, IRelatorioSemanalRepository
    {
        #region Construtor
        public RelatorioSemanalRepository(ListaDeTarefaContexto context) : base(context) { }
        #endregion Construtor

        public async Task<RelatorioSemanal?> BuscarPorInicioSemana(DateTimeOffset inicioSemana)
        {
            try
            {
                var inicioUtc = inicioSemana.ToUniversalTime();
                return await _dbSet.FirstOrDefaultAsync(r => r.InicioSemana == inicioUtc);
            }
            catch (Exception ex)
            {
                throw new Exception(" ", ex);
            }
        }

        public async Task<RelatorioSemanal?> BuscarUltimo()
        {
            try
            {
                return await _dbSet.OrderByDescending(r => r.InicioSemana).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(" ", ex);
            }
        }
    }
}
