using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;
using ASPCTS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPCTS.DTOs.Relatorio;
using AutoMapper;
using System.Security.Claims; // Necessário para ClaimTypes

namespace ASPCTS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Garante que todos os endpoints aqui exigem autenticação
    public class relatorioController : ControllerBase
    {
        private readonly IRelatorioService _relatorioService;
        private readonly IMapper _mapper;
        private readonly ICriancaService _criancaService;

        public relatorioController(IRelatorioService relatorioService, IMapper mapper, ICriancaService criancaService)
        {
            _relatorioService = relatorioService;
            _criancaService = criancaService;
            _mapper = mapper;
        }

        // GET: api/relatorio/buscar-todos-relatorios
        // Permite que psicólogos e responsáveis vejam apenas os relatórios de suas crianças
        [Authorize(Roles = "Psicologo")]
        [HttpGet("buscar-todos-relatorios")]
        [ProducesResponseType(typeof(IEnumerable<RelatorioDTO>), 200)]
        public async Task<ActionResult<IEnumerable<RelatorioDTO>>> GetAll()
        {
            var tipoUsuario = User.FindFirst(ClaimTypes.Role)?.Value;
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (string.IsNullOrEmpty(tipoUsuario) || usuarioId == 0)
            {
                return Unauthorized("Informações do usuário não encontradas no token.");
            }

            var relatoriosQuery = _relatorioService.GetQueryableRelatorios()
                                                  .Where(r => r.Ativo); // Sempre filtrar por relatórios ativos

            // Lógica de filtragem baseada no tipo de usuário
            if (tipoUsuario == "Psicologo")
            {
                relatoriosQuery = relatoriosQuery.Where(r => r.Crianca != null && r.Crianca.PsicologoId == usuarioId);
            }
            else if (tipoUsuario == "Responsavel")
            {
                relatoriosQuery = relatoriosQuery.Where(r => r.Crianca != null && (r.Crianca.PaiId == usuarioId || r.Crianca.MaeId == usuarioId));
            }
            else
            {
                // Se o tipo de usuário não for Psicólogo nem Responsável, não retorna nada.
                // Ou você pode retornar Forbid(), dependendo da sua política de segurança.
                return Forbid("Seu tipo de usuário não tem permissão para acessar relatórios.");
            }

            var relatorios = await relatoriosQuery.ToListAsync();
            var relatoriosDto = _mapper.Map<IEnumerable<RelatorioDTO>>(relatorios);
            return Ok(relatoriosDto);
        }

        // GET: api/relatorio/buscar-relatorio-por-id/{id}
        // Permite que psicólogos e responsáveis vejam um relatório específico, desde que vinculado
        [Authorize(Roles = "Psicologo")]
        [HttpGet("buscar-relatorio-por-id/{id}")]
        [ProducesResponseType(typeof(RelatorioDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<RelatorioDTO>> GetById(int id)
        {
            var relatorio = await _relatorioService.GetRelatorioByIdAsync(id);
            if (relatorio == null || !relatorio.Ativo) return NotFound("Relatório não encontrado ou inativo.");

            var tipoUsuario = User.FindFirst(ClaimTypes.Role)?.Value;
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (string.IsNullOrEmpty(tipoUsuario) || usuarioId == 0)
            {
                return Unauthorized("Informações do usuário não encontradas no token.");
            }

            var crianca = relatorio.Crianca;
            if (crianca == null) return NotFound("Informações da criança vinculada ao relatório não encontradas.");

            bool autorizado = false;
            if (tipoUsuario == "Psicologo" && crianca.PsicologoId == usuarioId)
            {
                autorizado = true;
            }
            else if (tipoUsuario == "Responsavel" && (crianca.PaiId == usuarioId || crianca.MaeId == usuarioId))
            {
                autorizado = true;
            }

            if (!autorizado)
            {
                return Forbid("Você não tem permissão para acessar este relatório.");
            }

            var relatorioDto = _mapper.Map<RelatorioDTO>(relatorio);
            return Ok(relatorioDto);
        }

        [HttpGet("buscar-relatorio-por-crianca-id/{criancaId}")]
        public async Task<IActionResult> GetRelatorioByCriancaIds(int criancaId)
        {
            var relatorios = await _relatorioService.GetRelatorioByCriancaIdAsync(criancaId);
            return Ok(relatorios);
        }

        // POST: api/relatorio/adicionar-relatorio
        // Exclusivamente para psicólogos
        [HttpPost("adicionar-relatorio")]
        [Authorize(Roles = "Psicologo")]
        [ProducesResponseType(typeof(RelatorioDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Create([FromBody] RelatorioCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var psicologoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (psicologoId == 0)
            {
                return Unauthorized("ID do psicólogo não encontrado no token.");
            }

            var crianca = await _criancaService.GetCriancaByIdAsync(dto.CriancaId);
            if (crianca == null)
            {
                return NotFound("Criança não encontrada.");
            }

            if (crianca.PsicologoId != psicologoId)
            {
                return Forbid("Você só pode criar relatórios para crianças sob sua responsabilidade.");
            }

            var relatorio = _mapper.Map<Relatorio>(dto);
            relatorio.CriancaId = dto.CriancaId; // Garante a vinculação
            // Data e Ativo serão definidos no AddRelatorioAsync() no repositório

            await _relatorioService.AddRelatorioAsync(relatorio);

            var relatorioResultDto = _mapper.Map<RelatorioDTO>(relatorio);
            return CreatedAtAction(nameof(GetById), new { id = relatorio.Id }, relatorioResultDto);
        }

        // PATCH: api/relatorio/atualizar-relatorio-por-id/{id}
        // Exclusivamente para psicólogos
        [HttpPatch("atualizar-relatorio-por-id/{id}")]
        [Authorize(Roles = "Psicologo")] // Apenas psicólogos podem atualizar
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update(int id, [FromBody] RelatorioUpdateDTO dto)
        {
            var relatorioExistente = await _relatorioService.GetRelatorioByIdAsync(id);
            if (relatorioExistente == null || !relatorioExistente.Ativo)
                return NotFound("Relatório não encontrado ou inativo.");

            var psicologoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (psicologoId == 0)
            {
                return Unauthorized("ID do psicólogo não encontrado no token.");
            }

            if (relatorioExistente.Crianca?.PsicologoId != psicologoId)
            {
                return Forbid("Apenas o psicólogo vinculado pode editar este relatório.");
            }

            // O AutoMapper cuida de mapear apenas as propriedades não nulas do DTO
            _mapper.Map(dto, relatorioExistente);

            await _relatorioService.UpdateRelatorioAsync(relatorioExistente);
            return NoContent();
        }

        // DELETE: api/relatorio/desativar-relatorio/{id}
        // Exclusivamente para psicólogos
        [HttpDelete("desativar-relatorio/{id}")]
        [Authorize(Roles = "Psicologo")] // Apenas psicólogos podem desativar
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Desativar(int id)
        {
            var relatorio = await _relatorioService.GetRelatorioByIdAsync(id);
            if (relatorio == null || !relatorio.Ativo)
                return NotFound("Relatório não encontrado ou já inativo.");

            var psicologoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (psicologoId == 0)
            {
                return Unauthorized("ID do psicólogo não encontrado no token.");
            }

            if (relatorio.Crianca?.PsicologoId != psicologoId)
                return Forbid("Apenas o psicólogo vinculado pode desativar este relatório.");

            await _relatorioService.DesativarRelatorioAsync(id); // Lógica de desativação já está no serviço/repositório
            return NoContent();
        }
    }
}