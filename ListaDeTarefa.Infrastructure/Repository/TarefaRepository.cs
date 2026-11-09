using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Application.DTOs.RelatorioSemanal;
using ListaDeTarefa.Infrastructure.Context;
using ListaDeTarefa.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ListaDeTarefa.Infrastructure.Repository
{
	public class TarefaRepository : GenericRepository<Tarefa>, ITarefaRepository
	{
		#region Construtor
		public TarefaRepository(ListaDeTarefaContexto context) : base(context) { }
		#endregion Construtor

		public async Task<Tarefa?> BuscarPorDescricao(string descricao)
		{
			try
			{
				return await _dbSet.FirstOrDefaultAsync(e => e.Descricao == descricao);
			}
			catch (Exception ex)
			{
				throw new Exception(" ", ex);
			}
		}

		public async Task<ResumoTarefasPeriodoDto> ObterResumoPorPeriodo(DateTimeOffset inicio, DateTimeOffset fim)
		{
			try
			{
				var totalCriadas = await _dbSet.CountAsync(t => t.DataCriacao >= inicio && t.DataCriacao < fim);

				var concluidasNoPeriodo = _dbSet.Where(t => t.DataConclusao != null && t.DataConclusao >= inicio && t.DataConclusao < fim);
				var totalConcluidas = await concluidasNoPeriodo.CountAsync();

				var totalPendentesAoFinal = await _dbSet.CountAsync(t => t.DataCriacao < fim && (t.DataConclusao == null || t.DataConclusao >= fim));

				double mediaHoras = totalConcluidas == 0
					? 0
					: await concluidasNoPeriodo.AverageAsync(t => (t.DataConclusao!.Value - t.DataCriacao).TotalHours);

				return new ResumoTarefasPeriodoDto(totalCriadas, totalConcluidas, totalPendentesAoFinal, Math.Round((decimal)mediaHoras, 2));
			}
			catch (Exception ex)
			{
				throw new Exception(" ", ex);
			}
		}

	}
}
