using ListaDeTarefa.Domain.Entities;

namespace ListaDeTarefa.Application.Interfaces.IRepository
{
	public interface ITarefaRepository : IGenericRepository<Tarefa>
	{
		Task<Tarefa?> BuscarPorDescricao(string descricao);
	}
}