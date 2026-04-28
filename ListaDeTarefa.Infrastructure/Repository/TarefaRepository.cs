using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Domain.Entities;
using ListaDeTarefa.Infrastructure.Context;
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

	}
}
