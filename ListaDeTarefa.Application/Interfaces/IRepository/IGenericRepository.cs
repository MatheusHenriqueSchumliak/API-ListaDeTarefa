using ListaDeTarefa.Domain.Entities.Base;

namespace ListaDeTarefa.Application.Interfaces.IRepository
{
	public interface IGenericRepository<T> where T : EntityBase
	{
		Task<IEnumerable<T>> BuscarTodos();
		Task<T?> BuscarPorId(Guid id);
		Task Adicionar(T entidade);
		Task Atualizar(T entidade);
		Task Remover(T entidade);
	}
}