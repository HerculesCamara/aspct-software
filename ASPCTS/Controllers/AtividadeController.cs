using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;
using ASPCTS.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASPCTS.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class AtividadeController : ControllerBase
    {
        private readonly IAtividadeService _atividadeService;
        public AtividadeController(IAtividadeService atividadeService)
        {
            _atividadeService = atividadeService;
        }

        //GET: api/atividade
        [HttpGet("BuscarTodasAtividades")]
        [ProducesResponseType(typeof(IEnumerable<Atividade>), 200)]
        public async Task<IActionResult> GetAllAtividades()
        {
            var atividades = await _atividadeService.GetAllAtividadesAsync();
            return Ok(atividades);
        }
        //GET: api/atividade/{id}
        [HttpGet("BuscarAtividadeId/{id}")]
        [ProducesResponseType(typeof(Atividade), 200)]
        public async Task<IActionResult> GetAtividadeById(int id)
        {
            var atividade = await _atividadeService.GetAtividadeByIdAsync(id);
            if (atividade == null)
            {
                return NotFound();
            }
            return Ok(atividade);
        }
        //POST: api/atividade
        [HttpPost("AdicionarAtividade")]
        [ProducesResponseType(typeof(Atividade), 201)]
        public async Task<IActionResult> AddAtividade([FromBody] Atividade atividade)
        {
            if (atividade == null)
            {
                return BadRequest("Atividade não pode ser nula.");
            }

            try
            {
                await _atividadeService.AddAtividadeAsync(atividade);
                return CreatedAtAction(nameof(GetAtividadeById), new { id = atividade.Id }, atividade);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno ao adicionar a atividade.");
            }
        }

        [HttpPut("AtualizarAtividade/{id}")]
        public async Task<IActionResult> UpdateAtividade(int id, Atividade atividadeAtualizada)
        {
            if (atividadeAtualizada == null || id != atividadeAtualizada.Id)
            {
                return BadRequest("Atividade não encontrada ou IDs incompatíveis.");
            }

            var existingAtividade = await _atividadeService.GetAtividadeByIdAsync(id);
            if (existingAtividade == null)
            {
                return NotFound("Atividade não encontrada.");
            }

            // Verifique se o status de conclusão foi fornecido
            if (atividadeAtualizada.Concluida == null)
            {
                return BadRequest("O status de conclusão deve ser informado.");
            }

            // Atualização de campos com base nos valores fornecidos
            existingAtividade.Titulo = !string.IsNullOrWhiteSpace(atividadeAtualizada.Titulo) ? atividadeAtualizada.Titulo : existingAtividade.Titulo;
            existingAtividade.Descricao = !string.IsNullOrWhiteSpace(atividadeAtualizada.Descricao) ? atividadeAtualizada.Descricao : existingAtividade.Descricao;
            if (atividadeAtualizada.Concluida.HasValue)
            {
                existingAtividade.Concluida = atividadeAtualizada.Concluida.Value;
            }

            // Atualizando a atividade no banco de dados
            await _atividadeService.UpdateAtividadeAsync(existingAtividade);

            return NoContent(); // Retorna 204 - No Content, indicando que a atualização foi bem-sucedida.
        }

        //DELETE: api/atividade/{id}
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