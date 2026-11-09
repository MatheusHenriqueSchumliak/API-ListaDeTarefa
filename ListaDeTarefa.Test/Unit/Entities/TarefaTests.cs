using ListaDeTarefa.Domain.Enumerables;
using ListaDeTarefa.Domain.Entities;
using ListaDeTarefa.Domain.Events;
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
		public void Concluir_DeveRegistrarEventoTarefaConcluida()
		{
			// Arrange
			var tarefa = Tarefa.Criar("Minha tarefa");

			// Act
			tarefa.Concluir();

			// Assert
			var evento = Assert.Single(tarefa.Eventos.OfType<TarefaConcluidaEvento>());
			Assert.Equal(tarefa.Id, evento.TarefaId);
			Assert.Null(evento.PessoaId);
			Assert.Equal(tarefa.DataCriacao, evento.DataCriacao);
			Assert.Equal(tarefa.DataConclusao, evento.DataConclusao);
		}

		[Fact]
		public void Concluir_NaoDeveDuplicarEvento_QuandoTarefaJaConcluida()
		{
			// Arrange
			var tarefa = Tarefa.Criar("Minha tarefa");
			tarefa.Concluir();

			// Act
			Assert.Throws<InvalidOperationException>(() => tarefa.Concluir());

			// Assert
			Assert.Single(tarefa.Eventos.OfType<TarefaConcluidaEvento>());
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

		private static Pessoa CriarPessoa()
			=> Pessoa.Criar("Ana", "Silva", "ana@email.com", "11999999999", "11999999999", DateTimeOffset.UtcNow.AddYears(-30), default);

		[Fact]
		public void AtribuirPessoa_DeveRegistrarEventoTarefaAtribuida()
		{
			// Arrange
			var tarefa = Tarefa.Criar("Minha tarefa");
			var pessoa = CriarPessoa();

			// Act
			tarefa.AtribuirPessoa(pessoa);

			// Assert
			var evento = Assert.IsType<TarefaAtribuidaEvento>(Assert.Single(tarefa.Eventos));
			Assert.Equal(tarefa.Id, evento.TarefaId);
			Assert.Equal(pessoa.Id, evento.PessoaId);
			Assert.Equal("Minha tarefa", evento.Descricao);
		}

		[Fact]
		public void AtribuirPessoa_NaoDeveRegistrarEvento_QuandoPessoaNula()
		{
			// Arrange
			var tarefa = Tarefa.Criar("Minha tarefa");

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => tarefa.AtribuirPessoa(null!));
			Assert.Empty(tarefa.Eventos);
		}

		[Fact]
		public void LimparEventos_DeveEsvaziarALista()
		{
			// Arrange
			var tarefa = Tarefa.Criar("Minha tarefa");
			tarefa.AtribuirPessoa(CriarPessoa());

			// Act
			tarefa.LimparEventos();

			// Assert
			Assert.Empty(tarefa.Eventos);
		}
	}
}