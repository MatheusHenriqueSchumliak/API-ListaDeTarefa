using ListaDeTarefa.Application.DTOs.RelatorioSemanal;
using ListaDeTarefa.Application.Interfaces.IService;
using Microsoft.AspNetCore.Mvc;

namespace ListaDeTarefa.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RelatorioSemanalController(IRelatorioSemanalService relatorioSemanalService, TimeProvider timeProvider) : ControllerBase
    {
        private readonly IRelatorioSemanalService _relatorioSemanalService = relatorioSemanalService;
        private readonly TimeProvider _timeProvider = timeProvider;

        [HttpPost]
        public async Task<ActionResult<RelatorioSemanalDto>> Gerar()
        {
            var relatorio = await _relatorioSemanalService.GerarRelatorioSemanal(_timeProvider.GetUtcNow());
            return Ok(relatorio);
        }

        [HttpGet]
        public async Task<ActionResult<RelatorioSemanalDto>> Ultimo()
        {
            var relatorio = await _relatorioSemanalService.BuscarUltimo();

            if (relatorio == null)
                return NotFound();

            return Ok(relatorio);
        }
    }
}
