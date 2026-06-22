using ListaDeTarefa.Application.DTOs.Tarefa;
using ListaDeTarefa.Test.Fixtures;
using System.Net.Http.Json;
using System.Net;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Controller
{
	public class TarefaControllerTests : IClassFixture<CustomWebApplicationFactory>
	{
		private readonly HttpClient _client;

		public TarefaControllerTests(CustomWebApplicationFactory factory)
		{
			_client = factory.CreateClient();
		}

		[Fact]
		public async Task GetTodos_DeveRetornar200()
		{
			// Act
			var response = await _client.GetAsync("/api/Tarefa/BuscarTodos");

			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact]
		public async Task Post_Adicionar_DeveRetornar201()
		{
			// Arrange
			var descricaoUnica = $"Tarefa Teste {Guid.NewGuid()}";
			var dto = new TarefaCreateDto { Descricao = descricaoUnica };

			// Act
			var response = await _client.PostAsJsonAsync("/api/Tarefa/Adicionar", dto);

			// Assert
			Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public async Task Post_Adicionar_DeveRetornar400_DescricaoVazia()
		{
			// Arrange
			var dto = new TarefaCreateDto { Descricao = "" };

			// Act
			var response = await _client.PostAsJsonAsync("/api/Tarefa/Adicionar", dto);

			// Assert
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task Get_BuscarPorId_DeveRetornar404_ParaIdInexistente()
		{
			// Arrange
			var id = Guid.NewGuid();

			// Act
			var response = await _client.GetAsync($"/api/Tarefa/BuscarPorId/{id}");

			// Assert
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public async Task Get_BuscarPorDescricao_DeveRetornar404_ParaDescricaoInexistente()
		{
			// Act
			var response = await _client.GetAsync($"/api/Tarefa/BuscarPorDescricao/descricao-inexistente-{Guid.NewGuid()}");

			// Assert
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public async Task Put_Atualizar_DeveRetornar400_ParaIdVazio()
		{
			// Arrange
			var dto = new TarefaUpdateDto { Descricao = "Atualizada", Status = 'C' };

			// Act
			var response = await _client.PutAsJsonAsync($"/api/Tarefa/Atualizar/{Guid.Empty}", dto);

			// Assert
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task Delete_Remover_DeveRetornar400_ParaIdVazio()
		{
			// Act
			var response = await _client.DeleteAsync($"/api/Tarefa/Remover/{Guid.Empty}");

			// Assert
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}
	}
}