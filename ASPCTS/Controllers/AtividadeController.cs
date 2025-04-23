using System.Collections.Generic;
using System.Threading.Tasks;
using ASPCTS.DTOs;
using ASPCTS.DTOs.Atividade;
using ASPCTS.Models;
using ASPCTS.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASPCTS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtividadeController : ControllerBase
    {
        private readonly IAtividadeService _atividadeService;
        private readonly IMapper _mapper;

        public AtividadeController(IAtividadeService atividadeService, IMapper mapper)
        {
            _atividadeService = atividadeService;
            _mapper = mapper;
        }

        [HttpGet("BuscarTodasAtividades")]
        [ProducesResponseType(typeof(IEnumerable<AtividadeDTO>), 200)]
        public async Task<IActionResult> GetAllAtividades()
        {
            var atividades = await _atividadeService.GetAllAtividadesAsync();
            var atividadesDto = _mapper.Map<IEnumerable<AtividadeDTO>>(atividades);
            return Ok(atividadesDto);
        }

        [HttpGet("BuscarAtividadeId/{id}")]
        [ProducesResponseType(typeof(AtividadeDTO), 200)]
        public async Task<IActionResult> GetAtividadeById(int id)
        {
            var atividade = await _atividadeService.GetAtividadeByIdAsync(id);
            if (atividade == null)
            {
                return NotFound();
            }
            var atividadeDto = _mapper.Map<AtividadeDTO>(atividade);
            return Ok(atividadeDto);
        }

        [HttpPost("AdicionarAtividade")]
        [ProducesResponseType(typeof(AtividadeDTO), 201)]
        public async Task<IActionResult> AddAtividade([FromBody] AtividadeCreateDTO atividadeDto)
        {
            if (atividadeDto == null)
            {
                return BadRequest("Atividade não pode ser nula.");
            }

            var atividade = _mapper.Map<Atividade>(atividadeDto);
            await _atividadeService.AddAtividadeAsync(atividade);
            var atividadeResultDto = _mapper.Map<AtividadeDTO>(atividade);
            return CreatedAtAction(nameof(GetAtividadeById), new { id = atividade.Id }, atividadeResultDto);
        }

        [HttpPut("AtualizarAtividade/{id}")]
        public async Task<IActionResult> UpdateAtividade(int id, [FromBody] AtividadeUpdateDTO atividadeDto)
        {
            if (atividadeDto == null)
            {
                return BadRequest("Dados inválidos para atualização.");
            }

            var atividadeExistente = await _atividadeService.GetAtividadeByIdAsync(id);
            if (atividadeExistente == null)
            {
                return NotFound("Atividade não encontrada.");
            }

            _mapper.Map(atividadeDto, atividadeExistente);
            await _atividadeService.UpdateAtividadeAsync(atividadeExistente);
            return NoContent();
        }

        [HttpDelete("DeletarAtividade/{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteAtividade(int id)
        {
            var atividade = await _atividadeService.GetAtividadeByIdAsync(id);
            if (atividade == null)
            {
                return NotFound();
            }

            await _atividadeService.DeleteAtividadeAsync(id);
            return NoContent();
        }
    }
}
