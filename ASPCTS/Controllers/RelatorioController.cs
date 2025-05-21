using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;
using ASPCTS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASPCTS.Controllers
{
    [Route("[controller]")]
    public class relatorioController : Controller
    {
        private readonly IRelatorioService _relatorioService;

        public relatorioController(IRelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        // GET: api/relatorio
        [HttpGet("buscar-todos-relatorios")]
        [ProducesResponseType(typeof(IEnumerable<Relatorio>), 200)]
        [ProducesResponseType(typeof(Relatorio), 404)]
        public async Task<ActionResult<IEnumerable<Relatorio>>> GetAll()
        {
            var relatorios = await _relatorioService.GetAllRelatorioAsync();
            return Ok(relatorios);
        }

        // GET: api/relatorio/{id}
        [HttpGet("buscar-relatorio-por-id/{id}")]
        [ProducesResponseType(typeof(Relatorio), 200)]
        [ProducesResponseType(typeof(Relatorio), 404)]
        public async Task<ActionResult<Relatorio>> GetById(int id)
        {
            var relatorio = await _relatorioService.GetRelatorioByIdAsync(id);
            if (relatorio == null)
                return NotFound();

            return Ok(relatorio);
        }

        // POST: api/relatorio
        [HttpPost("adicionar-relatorio")]
        [ProducesResponseType(typeof(Relatorio), 201)]
        [ProducesResponseType(typeof(Relatorio), 400)]
        [ProducesResponseType(typeof(Relatorio), 404)]
        public async Task<ActionResult> Create([FromBody] Relatorio relatorio)
        {
            await _relatorioService.AddRelatorioAsync(relatorio);
            return CreatedAtAction(nameof(GetById), new { id = relatorio.Id }, relatorio);
        }

        // PUT: api/relatorio/{id}
        [HttpPut("atualizar-relatorio-por-id/{id}")]
        [ProducesResponseType(typeof(Relatorio), 204)]
        [ProducesResponseType(typeof(Relatorio), 400)]
        [ProducesResponseType(typeof(Relatorio), 404)]
        public async Task<ActionResult> Update(int id, [FromBody] Relatorio relatorio)
        {
            if (relatorio.Id != Guid.Parse(id.ToString()))
                return BadRequest("ID do relatório não confere.");

            await _relatorioService.UpdateRelatorioAsync(relatorio);
            return NoContent();
        }

        // DELETE: api/relatorio/{id}
        [HttpDelete("desativar-relatorio/{id}")]
        [ProducesResponseType(typeof(Relatorio), 204)]
        [ProducesResponseType(typeof(Relatorio), 404)]
        public async Task<ActionResult> Inativar(int id)
        {
            var relatorio = await _relatorioService.GetRelatorioByIdAsync(id);
            if (relatorio == null)
                return NotFound();

            await _relatorioService.DesativarRelatorioAsync(id);
            return NoContent();
        }
    }
}