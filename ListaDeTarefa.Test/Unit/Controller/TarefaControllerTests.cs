using ListaDeTarefa.Application.DTOs.Tarefa;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Controller
{
	public class TarefaControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly HttpClient _client = factory.CreateClient();

		[Fact]
		public async Task GetTodos_DeveRetornar200()
		{
			var response = await _client.GetAsync("/api/Tarefa/BuscarTodos");
			response.EnsureSuccessStatusCode();
		}

		[Fact]
		public async Task Post_Adicionar_DeveRetornar201()
		{
			var descricaoUnica = $"Tarefa Teste {Guid.NewGuid()}";
			var dto = new TarefaCreateDto { Descricao = descricaoUnica };
			var response = await _client.PostAsJsonAsync("/api/Tarefa/Adicionar", dto);

			Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		}

		[Fact]
		public async Task Post_Adicionar_DeveRetornar400_DescricaoVazia()
		{
			var dto = new TarefaCreateDto { Descricao = "" };
			var response = await _client.PostAsJsonAsync("/api/Tarefa/Adicionar", dto);

			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task Get_BuscarPorId_DeveRetornar404_ParaIdInexistente()
		{
			var id = Guid.NewGuid();
			var response = await _client.GetAsync($"/api/Tarefa/BuscarPorId/{id}");

			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public async Task Get_BuscarPorDescricao_DeveRetornar404_ParaDescricaoInexistente()
		{
			var response = await _client.GetAsync($"/api/Tarefa/BuscarPorDescricao/descricao-inexistente");

			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public async Task Put_Atualizar_DeveRetornar400_ParaIdVazio()
		{
			var dto = new TarefaUpdateDto { Descricao = "Atualizada", Status = 'C' };
			var response = await _client.PutAsJsonAsync($"/api/Tarefa/Atualizar/{Guid.Empty}", dto);

			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task Delete_Remover_DeveRetornar400_ParaIdVazio()
		{
			var response = await _client.DeleteAsync($"/api/Tarefa/Remover/{Guid.Empty}");

			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

	}
}
