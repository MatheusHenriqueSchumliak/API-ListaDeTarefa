using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Application.Interfaces.IService;
using ListaDeTarefa.Application.DTOs.Pessoa;

namespace ListaDeTarefa.Application.Services
{
	public class PessoaService(IPessoaRepository pessoaRepository) : IPessoaService
	{
		#region Construtor
		private readonly IPessoaRepository _pessoaRepository = pessoaRepository;
		#endregion Construtor

		public Task<PessoaDto> Adicionar(PessoaCreateDto createDto)
		{
			throw new NotImplementedException();
		}

		public Task<PessoaDto?> BuscarPorId(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<PessoaDto?> BuscarPorNome(string nome)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<PessoaDto>> BuscarTodos()
		{
			throw new NotImplementedException();
		}

		public Task Remover(Guid id)
		{
			throw new NotImplementedException();
		}
	}
}
