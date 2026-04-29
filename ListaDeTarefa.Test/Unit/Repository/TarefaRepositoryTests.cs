using ListaDeTarefa.Infrastructure.Context;
using ListaDeTarefa.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using ListaDeTarefa.Domain.Entities;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Repository
{
	public class TarefaRepositoryTests
	{
		private TarefaRepository CriarRepository()
		{
			var options = new DbContextOptionsBuilder<ListaDeTarefaContexto>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;
			var contexto = new ListaDeTarefaContexto(options);
			return new TarefaRepository(contexto);
		}

		[Fact]
		public async Task Adicionar_DeveAdicionarTarefa()
		{
			// Arrange
			var repository = CriarRepository();
			var tarefa = new Tarefa().Criar("Nova tarefa");

			// Act
			await repository.Adicionar(tarefa);
			var resultado = await repository.BuscarPorId(tarefa.Id);

			// Assert
			Assert.NotNull(resultado);
			Assert.Equal("Nova tarefa", resultado.Descricao);
		}

		[Fact]
		public async Task ObterTodos_DeveRetornarTodasTarefas()
		{
			// Arrange
			var repository = CriarRepository();
			await repository.Adicionar(new Tarefa().Criar("Tarefa 1"));
			await repository.Adicionar(new Tarefa().Criar("Tarefa 2"));

			// Act
			var tarefas = (await repository.BuscarTodos()).ToList();

			// Assert
			Assert.Equal(2, tarefas.Count);
		}

		[Fact]
		public async Task Remover_DeveRemoverTarefa()
		{
			// Arrange
			var repository = CriarRepository();
			var tarefa = new Tarefa().Criar("Tarefa para remover");
			await repository.Adicionar(tarefa);

			// Act
			await repository.Remover(tarefa);
			var resultado = await repository.BuscarPorId(tarefa.Id);

			// Assert
			Assert.Null(resultado);
		}

		[Fact]
		public async Task Atualizar_DeveAlterarDescricaoDaTarefa()
		{
			var repository = CriarRepository();
			var tarefa = new Tarefa().Criar("Tarefa original");
			await repository.Adicionar(tarefa);

			tarefa.AlterarDescricao("Tarefa alterada");
			await repository.Atualizar(tarefa);

			var resultado = await repository.BuscarPorId(tarefa.Id);
			Assert.Equal("Tarefa alterada", resultado.Descricao);
		}

		[Fact]
		public async Task BuscarPorId_DeveRetornarNullParaIdInexistente()
		{
			var repository = CriarRepository();
			var resultado = await repository.BuscarPorId(Guid.NewGuid());
			Assert.Null(resultado);
		}

		[Fact]
		public async Task BuscarPorDescricao_DeveRetornarTarefaCorreta()
		{
			var repository = CriarRepository();
			var tarefa = new Tarefa().Criar("Descrição única");
			await repository.Adicionar(tarefa);

			var resultado = await repository.BuscarPorDescricao("Descrição única");
			Assert.NotNull(resultado);
			Assert.Equal(tarefa.Id, resultado.Id);
		}

		[Fact]
		public async Task BuscarPorDescricao_DeveRetornarNullSeNaoEncontrar()
		{
			var repository = CriarRepository();
			var resultado = await repository.BuscarPorDescricao("Inexistente");
			Assert.Null(resultado);
		}

		[Fact]
		public async Task NaoPermiteAlterarDescricaoDeTarefaConcluida()
		{
			var repository = CriarRepository();
			var tarefa = new Tarefa().Criar("Tarefa para concluir");
			await repository.Adicionar(tarefa);

			tarefa.Concluir();
			await repository.Atualizar(tarefa);

			Assert.Throws<InvalidOperationException>(() => tarefa.AlterarDescricao("Nova descrição"));
		}
	}
}