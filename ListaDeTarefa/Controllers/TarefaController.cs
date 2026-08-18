using ListaDeTarefa.Application.Interfaces.IService;
using ListaDeTarefa.Application.DTOs.Tarefa;
using ListaDeTarefa.Application.Commom;
using Microsoft.AspNetCore.Mvc;

namespace ListaDeTarefa.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class TarefaController(ITarefaService tarefaService, ILogger<TarefaController> logger) : ControllerBase
	{
		private readonly ITarefaService _tarefaService = tarefaService;
		private readonly ILogger<TarefaController> _logger = logger;

		[HttpGet("{descricao}")]
		public async Task<ActionResult<TarefaDto>> BuscarPorDescricao(string descricao)
		{
			using var activity = TelemetryHelper.IniciaRequisicao(
				"TarefaController.BuscarPorDescricao",
				new Dictionary<string, object?>
				{
					["http.method"] = "GET",
					["http.route"] = "/api/tarefa/BuscarPorDescricao/{descricao}",
					["tarefa.descricao"] = descricao
				});

			_logger.LogInformation("Iniciando busca por descrição: {Descricao}", descricao);

			try
			{
				if (string.IsNullOrWhiteSpace(descricao))
				{
					_logger.LogWarning("Descrição vazia ou nula recebida");
					activity.AdicionaTag("validation.error", "empty_description");
					return BadRequest("A descrição não pode ser vazia.");
				}

				var tarefa = await _tarefaService.BuscarPorDescricao(descricao);

				if (tarefa == null)
				{
					_logger.LogInformation("Tarefa não encontrada com descrição: {Descricao}", descricao);
					activity.AdicionaTags(new Dictionary<string, object?>
					{
						["result.found"] = false,
						["resource.type"] = "Tarefa"
					});
					activity.RegistraSucesso("Busca concluída - nenhum registro encontrado");
					return NotFound();
				}

				_logger.LogInformation("Tarefa encontrada: {TarefaId}", tarefa.Id);
				activity.AdicionaTags(new Dictionary<string, object?>
				{
					["tarefa.id"] = tarefa.Id,
					["result.found"] = true
				});
				activity.RegistraSucesso();

				return Ok(tarefa);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erro ao buscar tarefa por descrição: {Descricao}", descricao);
				activity.RegistraErro(ex);
				throw;
			}
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<TarefaDto>>> BuscarTodos()
		{
			var tarefas = await _tarefaService.BuscarTodos();
			return Ok(tarefas);
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<TarefaDto>> BuscarPorId(Guid id)
		{
			if (id == Guid.Empty)
				return BadRequest("Id inválido.");

			var tarefa = await _tarefaService.BuscarPorId(id);

			if (tarefa == null)
				return NotFound();

			return Ok(tarefa);
		}

		[HttpPost]
		public async Task<ActionResult<TarefaDto>> Adicionar([FromBody] TarefaCreateDto dto)
		{
			if (dto == null)
				return BadRequest("Dados obrigatórios não informados.");

			if (string.IsNullOrWhiteSpace(dto.Descricao))
				return BadRequest("A descrição é obrigatória.");

			var tarefa = await _tarefaService.Adicionar(dto);

			return CreatedAtAction(nameof(BuscarPorId), new { id = tarefa.Id }, tarefa);
		}

		[HttpPut("{id:guid}")]
		public async Task<IActionResult> Atualizar(Guid id, [FromBody] TarefaUpdateDto dto)
		{
			if (id == Guid.Empty)
				return BadRequest("Id inválido.");

			if (dto == null)
				return BadRequest("Dados obrigatórios não informados.");

			if (string.IsNullOrWhiteSpace(dto.Descricao))
				return BadRequest("A descrição é obrigatória.");

			await _tarefaService.Atualizar(id, dto);

			return NoContent();
		}

		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> Remover(Guid id)
		{
			if (id == Guid.Empty)
				return BadRequest("Id inválido.");

			await _tarefaService.Remover(id);

			return NoContent();
		}

	}
}