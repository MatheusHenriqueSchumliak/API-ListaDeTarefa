using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Domain.Entities.Base;
using ListaDeTarefa.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ListaDeTarefa.Infrastructure.Repository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : EntityBase
	{
		#region Construtor
		protected readonly ListaDeTarefaContexto _context;
		protected readonly DbSet<T> _dbSet;

		public GenericRepository(ListaDeTarefaContexto context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}
		#endregion Construtor

		public virtual async Task<IEnumerable<T>> BuscarTodos()
		{
			try
			{
				return await _dbSet.ToListAsync();
			}
			catch (Exception ex)
			{
				return Enumerable.Empty<T>();
			}
		}

		public virtual async Task<T?> BuscarPorId(Guid id)
		{
			try
			{
				return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
			}
			catch (Exception ex)
			{
				throw new Exception(" ", ex);
			}
		}

		public virtual async Task Adicionar(T entidade)
		{
			try
			{
				await _dbSet.AddAsync(entidade);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(" ", ex);
			}
		}

		public virtual async Task Atualizar(T entidade)
		{
			try
			{
				_dbSet.Update(entidade);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(" ", ex);
			}
		}

		public virtual async Task Remover(T entidade)
		{
			try
			{
				_dbSet.Remove(entidade);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(" ", ex);
			}
		}

	}
}
