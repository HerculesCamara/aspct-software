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
    public class atividadeController : ControllerBase
    {
        private readonly IAtividadeService _atividadeService;
        private readonly IMapper _mapper;

        public atividadeController(IAtividadeService atividadeService, IMapper mapper)
        {
            _atividadeService = atividadeService;
            _mapper = mapper;
        }

        [HttpGet("buscar-atividades")]
        [ProducesResponseType(typeof(IEnumerable<AtividadeDTO>), 200)]
        public async Task<IActionResult> GetAllAtividades()
        {
            var atividades = await _atividadeService.GetAllAtividadesAsync();
            var atividadesDto = _mapper.Map<IEnumerable<AtividadeDTO>>(atividades);
            return Ok(atividadesDto);
        }

        [HttpGet("buscar-atividade-id/{id}")]
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

        [HttpPost("adicionar-atividade")]
        [ProducesResponseType(typeof(AtividadeDTO), 400)]
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

        [HttpPatch("atualizar-atividade/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AtualizarAtividadeParcial(int id, [FromBody] AtividadeUpdateDTO dto)
        {
            var atividade = await _atividadeService.GetAtividadeByIdAsync(id);
            if (atividade == null)
            {
                return NotFound();
            }

            // Atualiza apenas os campos fornecidos
            if (!string.IsNullOrEmpty(dto.Titulo))
                atividade.Titulo = dto.Titulo;

            if (!string.IsNullOrEmpty(dto.Descricao))
                atividade.Descricao = dto.Descricao;

            if (dto.Concluida.HasValue)
            {
                atividade.Concluida = dto.Concluida.Value;

                // Se a atividade for marcada como concluída, define a data de conclusão como a data atual
                if (atividade.Concluida.Value)
                {
                    atividade.DataConclusao = DateTime.UtcNow;
                }
            }

            if (dto.DataConclusao.HasValue)
                atividade.DataConclusao = dto.DataConclusao;


            // Chama o serviço para atualizar a atividade no repositório
            await _atividadeService.UpdateAtividadeAsync(atividade);

            return NoContent();
        }


        [HttpDelete("desativar-atividade/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DesativarAtividade(int id)
        {
            var atividade = await _atividadeService.GetAtividadeByIdAsync(id);
            if (atividade == null)
            {
                return NotFound();
            }

            await _atividadeService.DesativarAtividadeAsync(id);
            return NoContent();
        }

    }
}
