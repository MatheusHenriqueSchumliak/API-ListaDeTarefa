using ListaDeTarefa.Application.DTOs.Tarefa;
using ListaDeTarefa.Domain.Enumerables;
using ListaDeTarefa.Domain.Entities;

namespace ListaDeTarefa.Application.Factories
{
	public static class TarefaFactory
	{
		public static TarefaDto EntidadeParaDto(Tarefa tarefa) => new()
		{
			Id = tarefa.Id,
			Descricao = tarefa.Descricao,
			Status = (char)tarefa.Status,
			DataCriacao = tarefa.DataCriacao,
			DataAtualizacao = tarefa.DataAtualizacao,
			DataConclusao = tarefa.DataConclusao
		};

		public static Tarefa createDtoParaEntidade(TarefaCreateDto dto)
		{
			return Tarefa.Criar(dto.Descricao);
		}

		public static void UpdateDtoParaEntidade(Tarefa tarefa, TarefaUpdateDto dto)
		{
			tarefa.AlterarDescricao(dto.Descricao);
			if (dto.Status == (char)StatusTarefa.Concluido && tarefa.Status != StatusTarefa.Concluido)
				tarefa.Concluir();
		}
	}
}
