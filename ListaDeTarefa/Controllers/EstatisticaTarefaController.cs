using ListaDeTarefa.Application.DTOs.EstatisticaTarefa;
using ListaDeTarefa.Application.Interfaces.IService;
using Microsoft.AspNetCore.Mvc;

namespace ListaDeTarefa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstatisticaTarefaController(IEstatisticaTarefaService estatisticaTarefaService) : ControllerBase
    {
        private readonly IEstatisticaTarefaService _estatisticaTarefaService = estatisticaTarefaService;

        [HttpGet("{tarefaId:guid}")]
        public async Task<ActionResult<EstatisticaTarefaDto>> BuscarPorTarefaId(Guid tarefaId)
        {
            if (tarefaId == Guid.Empty)
                return BadRequest("Id da tarefa inválido.");

            var estatistica = await _estatisticaTarefaService.BuscarPorTarefaId(tarefaId);

            if (estatistica is null)
                return NotFound();

            return Ok(estatistica);
        }
    }
}
