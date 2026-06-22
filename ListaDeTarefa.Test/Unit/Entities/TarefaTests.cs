using ListaDeTarefa.Domain.Enumerables;
using ListaDeTarefa.Domain.Entities;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Entities
{
	public class TarefaTests
	{
		[Fact]
		public void Criar_DeveRetornarTarefaComStatusPendente()
		{
			// Arrange & Act
			var tarefa = Tarefa.Criar("Minha tarefa");

			// Assert
			Assert.NotNull(tarefa);
			Assert.Equal("Minha tarefa", tarefa.Descricao);
			Assert.Equal(StatusTarefa.Pendente, tarefa.Status);
			Assert.Null(tarefa.DataConclusao);
		}

		[Fact]
		public void Criar_DeveLancarExcecao_QuandoDescricaoVazia()
		{
			// Act & Assert
			Assert.Throws<ArgumentException>(() => Tarefa.Criar(""));
		}

		[Fact]
		public void Concluir_DeveMarcarTarefaComoConcluida()
		{
			// Arrange
			var tarefa = Tarefa.Criar("Minha tarefa");

			// Act
			tarefa.Concluir();

			// Assert
			Assert.Equal(StatusTarefa.Concluido, tarefa.Status);
			Assert.NotNull(tarefa.DataConclusao);
		}

		[Fact]
		public void Concluir_DeveLancarExcecao_QuandoTarefaJaConcluida()
		{
			// Arrange
			var tarefa = Tarefa.Criar("Minha tarefa");
			tarefa.Concluir();

			// Act & Assert
			var exception = Assert.Throws<InvalidOperationException>(() => tarefa.Concluir());
			Assert.Equal("Tarefa já está concluída.", exception.Message);
		}

		[Fact]
		public void AlterarDescricao_DeveAtualizarDescricao()
		{
			// Arrange
			var tarefa = Tarefa.Criar("Descrição antiga");

			// Act
			tarefa.AlterarDescricao("Descrição nova");

			// Assert
			Assert.Equal("Descrição nova", tarefa.Descricao);
			Assert.NotNull(tarefa.DataAtualizacao);
		}

		[Fact]
		public void AlterarDescricao_DeveLancarExcecao_QuandoTarefaConcluida()
		{
			// Arrange
			var tarefa = Tarefa.Criar("Minha tarefa");
			tarefa.Concluir();

			// Act & Assert
			var exception = Assert.Throws<InvalidOperationException>(() => tarefa.AlterarDescricao("Nova descrição"));
			Assert.Equal("Não é possível alterar uma tarefa concluída.", exception.Message);
		}

		[Fact]
		public void AlterarDescricao_DeveLancarExcecao_QuandoDescricaoVazia()
		{
			// Arrange
			var tarefa = Tarefa.Criar("Minha tarefa");

			// Act & Assert
			Assert.Throws<ArgumentException>(() => tarefa.AlterarDescricao(""));
		}
	}
}