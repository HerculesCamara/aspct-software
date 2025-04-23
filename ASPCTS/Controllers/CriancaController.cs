using ASPCTS.DTOs;
using ASPCTS.Models;
using ASPCTS.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ASPCTS.DTOs.Crianca;

namespace ASPCTS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CriancaController : ControllerBase
    {
        private readonly ICriancaService _criancaService;
        private readonly IMapper _mapper;

        public CriancaController(ICriancaService criancaService, IMapper mapper)
        {
            _criancaService = criancaService;
            _mapper = mapper;
        }

        [HttpGet("BuscarTodasCriancas")]
        [ProducesResponseType(typeof(IEnumerable<CriancaDTO>), 200)]
        public async Task<IActionResult> GetAllCriancas()
        {
            var criancas = await _criancaService.GetAllCriancasAsync();
            var criancasDto = _mapper.Map<IEnumerable<CriancaDTO>>(criancas);
            return Ok(criancasDto);
        }

        [HttpGet("BuscarCriancaId/{id}")]
        [ProducesResponseType(typeof(CriancaDTO), 200)]
        public async Task<IActionResult> GetCriancaById(int id)
        {
            var crianca = await _criancaService.GetCriancaByIdAsync(id);
            if (crianca == null)
                return NotFound();

            var criancaDto = _mapper.Map<CriancaDTO>(crianca);
            return Ok(criancaDto);
        }

        [HttpPost("AdicionarCrianca")]
        [ProducesResponseType(typeof(CriancaDTO), 201)]
        public async Task<IActionResult> AddCrianca([FromBody] CriancaCreateDTO novaCriancaDto)
        {
            if (novaCriancaDto == null)
                return BadRequest("Dados inválidos.");

            var crianca = _mapper.Map<Crianca>(novaCriancaDto);
            await _criancaService.AddCriancaAsync(crianca);

            var criancaCriada = _mapper.Map<CriancaDTO>(crianca);
            return CreatedAtAction(nameof(GetCriancaById), new { id = crianca.Id }, criancaCriada);
        }

        [HttpPut("AtualizarCrianca/{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateCrianca(int id, [FromBody] CriancaUpdateDTO criancaDto)
        {
            var criancaExistente = await _criancaService.GetCriancaByIdAsync(id);
            if (criancaExistente == null)
                return NotFound("Criança não encontrada.");

            _mapper.Map(criancaDto, criancaExistente);
            await _criancaService.UpdateCriancaAsync(criancaExistente);

            return NoContent();
        }

        [HttpDelete("DeletarCrianca/{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteCrianca(int id)
        {
            var crianca = await _criancaService.GetCriancaByIdAsync(id);
            if (crianca == null)
                return NotFound();

            await _criancaService.DeleteCriancaAsync(id);
            return NoContent();
        }
    }
}
