using ListaDeTarefa.Application.DTOs.Pessoa;
using ListaDeTarefa.Application.Interfaces.IService;
using Microsoft.AspNetCore.Mvc;

namespace ListaDeTarefa.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PessoaController(IPessoaService pessoaService) : ControllerBase
    {
        private readonly IPessoaService _pessoaService = pessoaService;

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PessoaDto>> BuscarPorId(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id inválido.");

            var pessoa = await _pessoaService.BuscarPorId(id);

            if (pessoa == null)
                return NotFound();

            return Ok(pessoa);
        }

        [HttpPost]
        public async Task<ActionResult<PessoaDto>> Adicionar([FromBody] PessoaCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Dados obrigatórios não informados.");

            try
            {
                var pessoa = await _pessoaService.Adicionar(dto);
                return CreatedAtAction(nameof(BuscarPorId), new { id = pessoa.Id }, pessoa);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
