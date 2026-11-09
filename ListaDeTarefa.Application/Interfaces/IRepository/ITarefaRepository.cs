using ListaDeTarefa.Application.DTOs.RelatorioSemanal;
using ListaDeTarefa.Domain.Entities;

namespace ListaDeTarefa.Application.Interfaces.IRepository
{
	public interface ITarefaRepository : IGenericRepository<Tarefa>
	{
		Task<Tarefa?> BuscarPorDescricao(string descricao);
		Task<ResumoTarefasPeriodoDto> ObterResumoPorPeriodo(DateTimeOffset inicio, DateTimeOffset fim);
	}
}