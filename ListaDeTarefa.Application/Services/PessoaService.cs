using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Application.Interfaces.IService;
using ListaDeTarefa.Application.DTOs.Pessoa;
using ListaDeTarefa.Application.Factories;

namespace ListaDeTarefa.Application.Services
{
	public class PessoaService(IPessoaRepository pessoaRepository) : IPessoaService
	{
		#region Construtor
		private readonly IPessoaRepository _pessoaRepository = pessoaRepository;
		#endregion Construtor

		public async Task<PessoaDto> Adicionar(PessoaCreateDto createDto)
		{
			var pessoa = PessoaFactory.CreateDtoParaEntidade(createDto);
			await _pessoaRepository.Adicionar(pessoa);
			return PessoaFactory.EntidadeParaDto(pessoa);
		}

		public async Task<PessoaDto?> BuscarPorId(Guid id)
		{
			var pessoa = await _pessoaRepository.BuscarPorId(id);
			return pessoa is null ? null : PessoaFactory.EntidadeParaDto(pessoa);
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
