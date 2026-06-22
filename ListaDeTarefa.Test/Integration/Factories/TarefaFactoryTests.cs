using ListaDeTarefa.Application.DTOs.Tarefa;
using ListaDeTarefa.Application.Factories;
using ListaDeTarefa.Domain.Enumerables;
using ListaDeTarefa.Domain.Entities;
using Xunit;

namespace ListaDeTarefa.Test.Integration.Factories
{
	public class TarefaFactoryTests
	{
		[Fact]
		public void EntidadeParaDto_DeveMapearCorretamente()
		{
			var tarefa = Tarefa.Criar("Teste");
			tarefa.Concluir();

			var dto = TarefaFactory.EntidadeParaDto(tarefa);

			Assert.Equal(tarefa.Id, dto.Id);
			Assert.Equal(tarefa.Descricao, dto.Descricao);
			Assert.Equal((char)tarefa.Status, dto.Status);
			Assert.Equal(tarefa.DataCriacao, dto.DataCriacao);
			Assert.Equal(tarefa.DataAtualizacao, dto.DataAtualizacao);
			Assert.Equal(tarefa.DataConclusao, dto.DataConclusao);
		}

		[Fact]
		public void CreateDtoParaEntidade_DeveCriarComDescricaoCorreta()
		{
			var dto = new TarefaCreateDto { Descricao = "Nova tarefa" };
			var tarefa = TarefaFactory.createDtoParaEntidade(dto);

			Assert.Equal(dto.Descricao, tarefa.Descricao);
			Assert.Equal(StatusTarefa.Pendente, tarefa.Status);
		}

		[Fact]
		public void UpdateDtoParaEntidade_DeveAtualizarDescricaoEConcluir()
		{
			var tarefa = Tarefa.Criar("Antiga");
			var dto = new TarefaUpdateDto { Descricao = "Nova", Status = (char)StatusTarefa.Concluido };

			TarefaFactory.UpdateDtoParaEntidade(tarefa, dto);

			Assert.Equal("Nova", tarefa.Descricao);
			Assert.Equal(StatusTarefa.Concluido, tarefa.Status);
			Assert.NotNull(tarefa.DataConclusao);
		}

		[Fact]
		public void UpdateDtoParaEntidade_DeveAtualizarApenasDescricao_SeStatusNaoConcluido()
		{
			var tarefa = Tarefa.Criar("Antiga");
			var dto = new TarefaUpdateDto { Descricao = "Nova", Status = (char)StatusTarefa.Pendente };

			TarefaFactory.UpdateDtoParaEntidade(tarefa, dto);

			Assert.Equal("Nova", tarefa.Descricao);
			Assert.Equal(StatusTarefa.Pendente, tarefa.Status);
			Assert.Null(tarefa.DataConclusao);
		}

		[Fact]
		public void CreateDtoParaEntidade_DeveLancarExcecao_DescricaoVazia()
		{
			var dto = new TarefaCreateDto { Descricao = "" };
			Assert.Throws<ArgumentException>(() => TarefaFactory.createDtoParaEntidade(dto));
		}

		[Fact]
		public void UpdateDtoParaEntidade_DeveLancarExcecao_DescricaoVazia()
		{
			var tarefa = Tarefa.Criar("Antiga");
			var dto = new TarefaUpdateDto { Descricao = "", Status = (char)StatusTarefa.Pendente };
			Assert.Throws<ArgumentException>(() => TarefaFactory.UpdateDtoParaEntidade(tarefa, dto));
		}

	}
}