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
    public class criancaController : ControllerBase
    {
        private readonly ICriancaService _criancaService;
        private readonly IMapper _mapper;

        public criancaController(ICriancaService criancaService, IMapper mapper)
        {
            _criancaService = criancaService;
            _mapper = mapper;
        }

        [HttpGet("buscar-todas-criancas")]
        [ProducesResponseType(typeof(IEnumerable<CriancaDTO>), 200)]
        public async Task<IActionResult> GetAllCriancas()
        {
            var criancas = await _criancaService.GetAllCriancasAsync();
            var criancasDto = _mapper.Map<IEnumerable<CriancaDTO>>(criancas);
            return Ok(criancasDto);
        }

        [HttpGet("buscar-crianca-por-id/{id}")]
        [ProducesResponseType(typeof(CriancaDTO), 200)]
        public async Task<IActionResult> GetCriancaById(int id)
        {
            var crianca = await _criancaService.GetCriancaByIdAsync(id);
            if (crianca == null)
                return NotFound();

            var criancaDto = _mapper.Map<CriancaDTO>(crianca);
            return Ok(criancaDto);
        }

        [HttpPost("adicionar-crianca")]
        [ProducesResponseType(typeof(CriancaDTO), 400)]
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

        [HttpPatch("atualizar-crianca/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AtualizarCriancaParcial(int id, [FromBody] CriancaUpdateDTO criancaDto)
        {
            var criancaExistente = await _criancaService.GetCriancaByIdAsync(id);
            if (criancaExistente == null)
                return NotFound("Criança não encontrada.");

            // Atualiza apenas os campos que foram enviados no DTO
            if (!string.IsNullOrWhiteSpace(criancaDto.Nome))
                criancaExistente.Nome = criancaDto.Nome;

            if (criancaDto.DataNascimento.HasValue)
                criancaExistente.DataNascimento = criancaDto.DataNascimento.Value;

            if (criancaDto.PaiId.HasValue)
                criancaExistente.PaiId = criancaDto.PaiId.Value;

            if (criancaDto.PsicologoId.HasValue)
                criancaExistente.PsicologoId = criancaDto.PsicologoId.Value;

            await _criancaService.UpdateCriancaAsync(criancaExistente);

            return NoContent();
        }


        [HttpDelete("desativar-crianca/{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteCrianca(int id)
        {
            var crianca = await _criancaService.GetCriancaByIdAsync(id);
            if (crianca == null)
                return NotFound();

            await _criancaService.DesativarCriancaAsync(id);
            return NoContent();
        }
    }
}
