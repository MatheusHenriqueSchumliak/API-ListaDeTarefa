using ListaDeTarefa.Application.DTOs.Tarefa;

namespace ListaDeTarefa.Application.Interfaces.IService
{
	public interface ITarefaService
	{
		Task<IEnumerable<TarefaDto>> BuscarTodos();
		Task<TarefaDto?> BuscarPorId(Guid id);
		Task<TarefaDto> Adicionar(TarefaCreateDto createDto);
		Task Atualizar(Guid id, TarefaUpdateDto updateDto);
		Task Remover(Guid id);

		Task<TarefaDto?> BuscarPorDescricao(string descricao);
	}
}
