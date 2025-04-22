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
    public class PsicologoController : ControllerBase
    {
        private readonly IPsicologoService _psicologoService;
        public PsicologoController(IPsicologoService psicologoService)
        {
            _psicologoService = psicologoService;
        }

        //GET: api/psicologo
        [HttpGet("BuscarTodosPsicologos")]
        [ProducesResponseType(typeof(IEnumerable<Psicologo>), 200)]
        public async Task<IActionResult> GetAllPsicologos()
        {
            var psicologos = await _psicologoService.GetAllPsicologosAsync();
            return Ok(psicologos);
        }

        //GET: api/psicologo/{id}
        [HttpGet("BuscarPorPsicologoId/{id}")]
        [ProducesResponseType(typeof(Psicologo), 200)]
        public async Task<IActionResult> GetPsicologoById(int id)
        {
            var psicologo = await _psicologoService.GetPsicologoByIdAsync(id);
            if (psicologo == null)
            {
                return NotFound();
            }
            return Ok(psicologo);
        }

        //POST: api/psicologo
        [HttpPost("AdicionarPsicologo")]
        [ProducesResponseType(typeof(Psicologo), 201)]
        public async Task<IActionResult> AddPsicologo(Psicologo psicologo)
        {
            if (psicologo == null)
            {
                return BadRequest("Psicologo object is null");
            }

            psicologo.Tipo = "Psicologo";

            var existingPsicologo = await _psicologoService.GetPsicologoByCPFAsync(psicologo.CPF);
            if (existingPsicologo != null)
            {
                return Conflict("A psicologo with this CPF already exists.");
            }

            await _psicologoService.AddPsicologoAsync(psicologo);
            return CreatedAtAction(nameof(GetPsicologoById), new { id = psicologo.Id }, psicologo);
        }

        // PUT: api/psicologo/{id}
[HttpPut("AtualizarPsicologo/{id}")]
[ProducesResponseType(typeof(Psicologo), 204)]
public async Task<IActionResult> UpdatePsicologo(int id, Psicologo psicologoAtualizado)
{
    if (psicologoAtualizado == null || id != psicologoAtualizado.Id)
    {
        return BadRequest("Psicologo não encontrado ou IDs incompatíveis.");
    }

    var existingPsicologo = await _psicologoService.GetPsicologoByIdAsync(id);
    if (existingPsicologo == null)
    {
        return NotFound("Psicologo não encontrado.");
    }

    // Atualização de campos com base nos valores fornecidos
    existingPsicologo.Name = !string.IsNullOrWhiteSpace(psicologoAtualizado.Name) ? psicologoAtualizado.Name : existingPsicologo.Name;
    existingPsicologo.CPF = !string.IsNullOrWhiteSpace(psicologoAtualizado.CPF) ? psicologoAtualizado.CPF : existingPsicologo.CPF;
    existingPsicologo.Phone = !string.IsNullOrWhiteSpace(psicologoAtualizado.Phone) ? psicologoAtualizado.Phone : existingPsicologo.Phone;
    existingPsicologo.Email = !string.IsNullOrWhiteSpace(psicologoAtualizado.Email) ? psicologoAtualizado.Email : existingPsicologo.Email;
    existingPsicologo.DataNascimento = psicologoAtualizado.DataNascimento != default(DateTimeOffset) ? psicologoAtualizado.DataNascimento : existingPsicologo.DataNascimento;
    existingPsicologo.Password = !string.IsNullOrWhiteSpace(psicologoAtualizado.Password) ? psicologoAtualizado.Password : existingPsicologo.Password;
    existingPsicologo.CRP = !string.IsNullOrWhiteSpace(psicologoAtualizado.CRP) ? psicologoAtualizado.CRP : existingPsicologo.CRP;

    // Atualizando o psicologo no banco de dados
    await _psicologoService.UpdatePsicologoAsync(existingPsicologo);

    return NoContent(); // Retorna 204 - No Content, indicando que a atualização foi bem-sucedida.
}


        //DELETE: api/psicologo/{id}
        [HttpDelete("DeletarPsicologo/{id}")]
        [ProducesResponseType(typeof(Psicologo), 204)]
        public async Task<IActionResult> DeletePsicologo(int id)
        {
            var psicologo = await _psicologoService.GetPsicologoByIdAsync(id);
            if (psicologo == null)
            {
                return NotFound();
            }
            await _psicologoService.DeletePsicologoAsync(id);
            return NoContent();
        }
    }
}