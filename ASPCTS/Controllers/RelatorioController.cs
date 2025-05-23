using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;
using ASPCTS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPCTS.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class relatorioController : Controller
    {
        private readonly IRelatorioService _relatorioService;

        public relatorioController(IRelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        // GET: relatorio/buscar-todos-relatorios
        [HttpGet("buscar-todos-relatorios")]
        public async Task<ActionResult<IEnumerable<Relatorio>>> GetAll()
        {
            var tipoUsuario = User.FindFirst("TipoUsuario")?.Value ?? string.Empty;
            var usuarioId = int.Parse(User.FindFirst("Id")?.Value ?? "0");

            var relatorios = await _relatorioService.GetQueryableRelatorios()
                .Where(r =>
                    (tipoUsuario == "Psicologo" && r.Crianca != null && r.Crianca.PsicologoId == usuarioId) ||
                    (tipoUsuario == "Responsavel" && r.Crianca != null && (r.Crianca.PaiId == usuarioId || r.Crianca.MaeId == usuarioId))
                ).ToListAsync();

            return Ok(relatorios);
        }

        // GET: relatorio/buscar-relatorio-por-id/{id}
        [HttpGet("buscar-relatorio-por-id/{id}")]
        public async Task<ActionResult<Relatorio>> GetById(int id)
        {
            var relatorio = await _relatorioService.GetRelatorioByIdAsync(id);
            if (relatorio == null) return NotFound();

            var tipoUsuario = User.FindFirst("TipoUsuario")?.Value ?? string.Empty;
            var usuarioId = int.Parse(User.FindFirst("Id")?.Value ?? "0");

            var crianca = relatorio.Crianca;

            bool autorizado = tipoUsuario switch
            {
                "Psicologo" => crianca != null && crianca.PsicologoId == usuarioId,
                "Responsavel" => crianca != null && (crianca.PaiId == usuarioId || crianca.MaeId == usuarioId),
                _ => false
            };

            if (!autorizado)
                return Forbid("Você não tem permissão para acessar este relatório.");

            return Ok(relatorio);
        }

        // POST: relatorio/adicionar-relatorio
        [HttpPost("adicionar-relatorio")]
        public async Task<ActionResult> Create([FromBody] Relatorio relatorio)
        {
            var tipoUsuario = User.FindFirst("TipoUsuario")?.Value ?? string.Empty;
            var usuarioId = int.Parse(User.FindFirst("Id")?.Value ?? "0");

            if (tipoUsuario != "Psicologo")
                return Forbid("Apenas psicólogos podem criar relatórios.");

            if (relatorio.Crianca?.PsicologoId != usuarioId)
                return Forbid("Você só pode criar relatórios para suas próprias crianças.");

            await _relatorioService.AddRelatorioAsync(relatorio);
            return CreatedAtAction(nameof(GetById), new { id = relatorio.Id }, relatorio);
        }

        // PATCH: relatorio/atualizar-relatorio-por-id/{id}
        [HttpPatch("atualizar-relatorio-por-id/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Relatorio relatorio)
        {
            var relatorioExistente = await _relatorioService.GetRelatorioByIdAsync(id);
            if (relatorioExistente == null)
                return NotFound();

            if (relatorio.Id != relatorioExistente.Id)
                return BadRequest("ID do relatório não confere.");

            var tipoUsuario = User.FindFirst("TipoUsuario")?.Value ?? string.Empty;
            var usuarioId = int.Parse(User.FindFirst("Id")?.Value ?? "0");

            if (tipoUsuario != "Psicologo" || relatorioExistente.Crianca == null || relatorioExistente.Crianca.PsicologoId != usuarioId)
                return Forbid("Apenas o psicólogo vinculado pode editar este relatório.");

            await _relatorioService.UpdateRelatorioAsync(relatorio);
            return NoContent();
        }

        // DELETE: relatorio/desativar-relatorio/{id}
        [HttpDelete("desativar-relatorio/{id}")]
        public async Task<ActionResult> Inativar(int id)
        {
            var relatorio = await _relatorioService.GetRelatorioByIdAsync(id);
            if (relatorio == null)
                return NotFound();

            var tipoUsuario = User.FindFirst("TipoUsuario")?.Value ?? string.Empty;
            var usuarioId = int.Parse(User.FindFirst("Id")?.Value ?? "0");

            if (tipoUsuario != "Psicologo" || relatorio.Crianca == null || relatorio.Crianca.PsicologoId != usuarioId)
                return Forbid("Apenas o psicólogo vinculado pode excluir este relatório.");

            await _relatorioService.DesativarRelatorioAsync(id);
            return NoContent();
        }
    }
}
