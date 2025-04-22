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
    public class CriancaController : ControllerBase
    {
        private readonly ICriancaService _criancaService;
        public CriancaController(ICriancaService criancaService)
        {
            _criancaService = criancaService;
        }

        //GET: api/crianca
        [HttpGet("BuscarTodasCriancas")]
        [ProducesResponseType(typeof(IEnumerable<Crianca>), 200)]
        public async Task<IActionResult> GetAllCriancas()
        {
            var criancas = await _criancaService.GetAllCriancasAsync();
            var criancasFiltro = criancas.Select(c => new
            {
                c.Id,
                c.Nome,
                c.DataNascimento,
                c.PaiId,
                c.PsicologoId
            }).ToList();
            return Ok(criancasFiltro);
        }

        //GET: api/crianca/{id}
        [HttpGet("BuscarCriancaId/{id}")]
        [ProducesResponseType(typeof(Crianca), 200)]
        public async Task<IActionResult> GetCriancaById(int id)
        {
            var crianca = await _criancaService.GetCriancaByIdAsync(id);
            if (crianca == null)
            {
                return NotFound();
            }
            var criancaFiltro = new
            {
                crianca.Id,
                crianca.Nome,
                crianca.DataNascimento,
                crianca.Idade,
                crianca.PaiId,
                crianca.PsicologoId
            };
            return Ok(criancaFiltro);
        }
        //POST: api/crianca
        [HttpPost("AdicionarCrianca")]
        [ProducesResponseType(typeof(Crianca), 201)]
        public async Task<IActionResult> AddCrianca([FromBody] Crianca crianca)
        {
            if (crianca == null)
            {
                return BadRequest("Crianca object is null");
            }

            var novaCrianca = new Crianca
            {
                Nome = crianca.Nome,
                DataNascimento = crianca.DataNascimento,
                PaiId = crianca.PaiId,
                PsicologoId = crianca.PsicologoId
            };
            await _criancaService.AddCriancaAsync(novaCrianca);
            return CreatedAtAction(nameof(GetCriancaById), new { id = novaCrianca.Id }, novaCrianca);
        }
        //PUT: api/crianca/{id}
        [HttpPut("AtualizarCrianca/{id}")]
        [ProducesResponseType(typeof(Crianca), 204)]
        public async Task<IActionResult> UpdateCrianca(int id, [FromBody] Crianca criancaAtualizada)
        {
            if (criancaAtualizada == null || id != criancaAtualizada.Id)
            {
                return BadRequest("Objeto Criança está nulo ou ID não corresponde.");
            }

            var existingCrianca = await _criancaService.GetCriancaByIdAsync(id);
            if (existingCrianca == null)
            {
                return NotFound("Criança não encontrada.");
            }

            // Atualiza somente os campos que vierem preenchidos
            if (!string.IsNullOrWhiteSpace(criancaAtualizada.Nome))
                existingCrianca.Nome = criancaAtualizada.Nome;

            if (criancaAtualizada.DataNascimento != default(DateTime))
            {
                existingCrianca.DataNascimento = criancaAtualizada.DataNascimento;
            }

            if (criancaAtualizada.PaiId != default(int))
                existingCrianca.PaiId = criancaAtualizada.PaiId;

            if (criancaAtualizada.PsicologoId != default(int))
                existingCrianca.PsicologoId = criancaAtualizada.PsicologoId;

            await _criancaService.UpdateCriancaAsync(existingCrianca);
            return NoContent();
        }

        //DELETE: api/crianca/{id}
        [HttpDelete("DeletarCrianca/{id}")]
        [ProducesResponseType(typeof(Crianca), 204)]
        public async Task<IActionResult> DeleteCrianca(int id)
        {
            var crianca = await _criancaService.GetCriancaByIdAsync(id);
            if (crianca == null)
            {
                return NotFound();
            }
            await _criancaService.DeleteCriancaAsync(id);
            return NoContent();
        }
    }
}