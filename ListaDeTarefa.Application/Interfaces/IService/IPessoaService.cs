using ListaDeTarefa.Application.DTOs.Pessoa;

namespace ListaDeTarefa.Application.Interfaces.IService
{
	public interface IPessoaService
	{
		Task<IEnumerable<PessoaDto>> BuscarTodos();
		Task<PessoaDto?> BuscarPorId(Guid id);
		Task<PessoaDto> Adicionar(PessoaCreateDto createDto);
		//Task Atualizar(Guid id, PessoaUpdateDto updateDto);
		Task Remover(Guid id);

		Task<PessoaDto?> BuscarPorNome(string nome);
	}
}
