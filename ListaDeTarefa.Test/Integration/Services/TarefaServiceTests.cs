using ListaDeTarefa.Application.DTOs.Tarefa;
using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Application.Services;
using ListaDeTarefa.Domain.Entities;
using ListaDeTarefa.Domain.Enumerables;
using Moq;
using Xunit;

namespace ListaDeTarefa.Test.Integration.Services
{
	public class TarefaServiceTests
	{
		[Fact]
		public async Task Adicionar_DeveLancarExcecao_QuandoDescricaoDuplicada()
		{
			// Arrange
			var mockRepo = new Mock<ITarefaRepository>();
			mockRepo.Setup(r => r.BuscarPorDescricao("Teste")).ReturnsAsync(new Tarefa());
			var service = new TarefaService(mockRepo.Object);

			var dto = new TarefaCreateDto { Descricao = "Teste" };

			// Act & Assert
			await Assert.ThrowsAsync<InvalidOperationException>(() => service.Adicionar(dto));
		}

		[Fact]
		public async Task Adicionar_DeveAdicionarComSucesso_QuandoDescricaoUnica()
		{
			// Arrange
			var mockRepo = new Mock<ITarefaRepository>();
			mockRepo.Setup(r => r.BuscarPorDescricao("Nova")).ReturnsAsync((Tarefa?)null);
			mockRepo.Setup(r => r.Adicionar(It.IsAny<Tarefa>())).Returns(Task.CompletedTask);
			var service = new TarefaService(mockRepo.Object);

			var dto = new TarefaCreateDto { Descricao = "Nova" };

			// Act
			var result = await service.Adicionar(dto);

			// Assert
			Assert.NotNull(result);
			Assert.Equal("Nova", result.Descricao);
		}

		[Fact]
		public async Task BuscarTodos_DeveRetornarListaDeTarefas()
		{
			// Arrange
			var tarefas = new List<Tarefa>
			{
				Tarefa.Criar("Tarefa 1"),
				Tarefa.Criar("Tarefa 2")
			};
			var mockRepo = new Mock<ITarefaRepository>();
			mockRepo.Setup(r => r.BuscarTodos()).ReturnsAsync(tarefas);
			var service = new TarefaService(mockRepo.Object);

			// Act
			var result = await service.BuscarTodos();

			// Assert
			Assert.Equal(2, result.Count());
		}

		[Fact]
		public async Task BuscarPorId_DeveRetornarTarefa_QuandoExiste()
		{
			// Arrange
			var tarefa = Tarefa.Criar("Buscar por Id");
			var mockRepo = new Mock<ITarefaRepository>();
			mockRepo.Setup(r => r.BuscarPorId(It.IsAny<Guid>())).ReturnsAsync(tarefa);
			var service = new TarefaService(mockRepo.Object);

			// Act
			var result = await service.BuscarPorId(Guid.NewGuid());

			// Assert
			Assert.NotNull(result);
			Assert.Equal("Buscar por Id", result.Descricao);
		}

		[Fact]
		public async Task BuscarPorId_DeveRetornarNull_QuandoNaoExiste()
		{
			// Arrange
			var mockRepo = new Mock<ITarefaRepository>();
			mockRepo.Setup(r => r.BuscarPorId(It.IsAny<Guid>())).ReturnsAsync((Tarefa?)null);
			var service = new TarefaService(mockRepo.Object);

			// Act
			var result = await service.BuscarPorId(Guid.NewGuid());

			// Assert
			Assert.Null(result);
		}

		[Fact]
		public async Task Atualizar_DeveLancarExcecao_QuandoTarefaNaoExiste()
		{
			// Arrange
			var mockRepo = new Mock<ITarefaRepository>();
			mockRepo.Setup(r => r.BuscarPorId(It.IsAny<Guid>())).ReturnsAsync((Tarefa?)null);
			var service = new TarefaService(mockRepo.Object);

			var dto = new TarefaUpdateDto { Descricao = "Atualizada", Status = (char)StatusTarefa.Concluido };

			// Act & Assert
			await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Atualizar(Guid.NewGuid(), dto));
		}

		[Fact]
		public async Task Atualizar_DeveLancarExcecao_QuandoDescricaoDuplicada()
		{
			// Arrange
			var tarefa = Tarefa.Criar("Original");
			var mockRepo = new Mock<ITarefaRepository>();
			mockRepo.Setup(r => r.BuscarPorId(It.IsAny<Guid>())).ReturnsAsync(tarefa);
			mockRepo.Setup(r => r.BuscarPorDescricao("Duplicada")).ReturnsAsync(new Tarefa { Id = Guid.NewGuid() });
			var service = new TarefaService(mockRepo.Object);

			var dto = new TarefaUpdateDto { Descricao = "Duplicada", Status = (char)StatusTarefa.Concluido };

			// Act & Assert
			await Assert.ThrowsAsync<InvalidOperationException>(() => service.Atualizar(Guid.NewGuid(), dto));
		}

		[Fact]
		public async Task Atualizar_DeveLancarExcecao_QuandoStatusNaoConcluido()
		{
			// Arrange
			var tarefa = Tarefa.Criar("Original");
			var mockRepo = new Mock<ITarefaRepository>();
			mockRepo.Setup(r => r.BuscarPorId(It.IsAny<Guid>())).ReturnsAsync(tarefa);
			mockRepo.Setup(r => r.BuscarPorDescricao("Original")).ReturnsAsync(tarefa);
			var service = new TarefaService(mockRepo.Object);

			var dto = new TarefaUpdateDto { Descricao = "Original", Status = (char)StatusTarefa.Pendente };

			// Act & Assert
			await Assert.ThrowsAsync<InvalidOperationException>(() => service.Atualizar(Guid.NewGuid(), dto));
		}

		[Fact]
		public async Task Remover_DeveLancarExcecao_QuandoTarefaNaoExiste()
		{
			// Arrange
			var mockRepo = new Mock<ITarefaRepository>();
			mockRepo.Setup(r => r.BuscarPorId(It.IsAny<Guid>())).ReturnsAsync((Tarefa?)null);
			var service = new TarefaService(mockRepo.Object);

			// Act & Assert
			await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Remover(Guid.NewGuid()));
		}

		[Fact]
		public async Task Remover_DeveRemoverComSucesso_QuandoTarefaExiste()
		{
			// Arrange
			var tarefa = Tarefa.Criar("Remover");
			var mockRepo = new Mock<ITarefaRepository>();
			mockRepo.Setup(r => r.BuscarPorId(It.IsAny<Guid>())).ReturnsAsync(tarefa);
			mockRepo.Setup(r => r.Remover(tarefa)).Returns(Task.CompletedTask);
			var service = new TarefaService(mockRepo.Object);

			// Act & Assert
			await service.Remover(Guid.NewGuid());
		}
	}
}
