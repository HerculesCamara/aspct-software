using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;
using ASPCTS.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace ASPCTS.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    public class PaiController : ControllerBase
    {
        private readonly IPaiService _paiService;
        public PaiController(IPaiService paiService)
        {
            _paiService = paiService;
        }

        //GET: api/pai
        [HttpGet("BuscarTodosPais")]
        [ProducesResponseType(typeof(IEnumerable<Pai>), 200)]
        public async Task<IActionResult> GetAllPais()
        {
            var pais = await _paiService.GetAllPaisAsync();
            return Ok(pais);
        }
        //GET: api/pai/{id}
        [HttpGet("BuscarPorPaiId/{id}")]
        [ProducesResponseType(typeof(Pai), 200)]
        public async Task<IActionResult> GetPaiById(int id)
        {
            var pai = await _paiService.GetPaiByIdAsync(id);
            if (pai == null)
            {
                return NotFound();
            }
            return Ok(pai);
        }
        //POST: api/pai
        [HttpPost("AdicionarPai")]
        [ProducesResponseType(typeof(Pai), 201)]
        public async Task<IActionResult> AddPai(Pai pai)
        {
            if (pai == null)
                return BadRequest("Objeto Pai está nulo.");

            // Garante que o tipo será sempre "Pai"
            pai.Tipo = "Pai";

            // Verifica se CPF já existe
            var existingPai = await _paiService.GetPaiByCPFAsync(pai.CPF);
            if (existingPai != null)
                return Conflict("Já existe um pai cadastrado com esse CPF.");

            await _paiService.AddPaiAsync(pai);

            return CreatedAtAction(nameof(GetPaiById), new { id = pai.Id }, pai);
        }
        //PUT: api/pai/{id}
        [HttpPut("AtualizarPai/{id}")]
        [ProducesResponseType(typeof(Pai), 204)]
        public async Task<IActionResult> UpdatePai(int id, Pai paiAtualizado)
        {
            if (paiAtualizado == null || id != paiAtualizado.Id)
            {
                return BadRequest("Pai não encontrado ou IDs incompatíveis.");
            }

            var existingPai = await _paiService.GetPaiByIdAsync(id);
            if (existingPai == null)
            {
                return NotFound("Pai não encontrado.");
            }

            // Atualização de campos com base nos valores fornecidos
            existingPai.Name = !string.IsNullOrWhiteSpace(paiAtualizado.Name) ? paiAtualizado.Name : existingPai.Name;
            existingPai.CPF = !string.IsNullOrWhiteSpace(paiAtualizado.CPF) ? paiAtualizado.CPF : existingPai.CPF;
            existingPai.Phone = !string.IsNullOrWhiteSpace(paiAtualizado.Phone) ? paiAtualizado.Phone : existingPai.Phone;
            existingPai.Email = !string.IsNullOrWhiteSpace(paiAtualizado.Email) ? paiAtualizado.Email : existingPai.Email;
            existingPai.DataNascimento = paiAtualizado.DataNascimento != default(DateTimeOffset) ? paiAtualizado.DataNascimento : existingPai.DataNascimento;
            existingPai.Password = !string.IsNullOrWhiteSpace(paiAtualizado.Password) ? paiAtualizado.Password : existingPai.Password;
            existingPai.PsicologoId = paiAtualizado.PsicologoId != default(int) ? paiAtualizado.PsicologoId : existingPai.PsicologoId;

            // Atualizando o pai no banco de dados
            await _paiService.UpdatePaiAsync(existingPai);

            return NoContent(); // Retorna 204 - No Content, indicando que a atualização foi bem-sucedida.
        }
        //DELETE: api/pai/{id}
        [HttpDelete("DeletarPai/{id}")]

        public async Task<IActionResult> DeletePai(int id)
        {
            var pai = await _paiService.GetPaiByIdAsync(id);
            if (pai == null)
            {
                return NotFound();
            }
            await _paiService.DeletePaiAsync(id);
            return NoContent();
        }
    }
}