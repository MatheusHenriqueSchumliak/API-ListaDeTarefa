using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Infrastructure.Context;
using ListaDeTarefa.Domain.Entities;

namespace ListaDeTarefa.Infrastructure.Repository
{
	public class PessoaRepository : GenericRepository<Pessoa>, IPessoaRepository
	{
		public PessoaRepository(ListaDeTarefaContexto context) : base(context) { }

	}
}
