using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ASPCTS.Context;
using ASPCTS.DTOs.Psicologo;
using ASPCTS.DTOs.Atividade;
using ASPCTS.Models;
using ASPCTS.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPCTS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PsicologoController : ControllerBase
    {
        private readonly IPsicologoService _psicologoService;
        private readonly IAtividadeService _atividadeService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public PsicologoController(IPsicologoService psicologoService, IAtividadeService atividadeService, IMapper mapper, ApplicationDbContext context)
        {
            _psicologoService = psicologoService;
            _atividadeService = atividadeService;
            _mapper = mapper;
            _context = context;
        }

        private int ObterUsuarioId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        private string ObterUsuarioRole() =>
            User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        [Authorize(Roles = "Psicologo")]
        [HttpGet("buscar-todos-psicologos")]
        public async Task<IActionResult> GetAllPsicologos()
        {
            var psicologosDto = await _psicologoService.GetAllPsicologosAsync();
            return Ok(psicologosDto);
        }

        [Authorize(Roles = "Psicologo")]
        [HttpGet("buscar-psicologo-por-id/{id}")]
        public async Task<IActionResult> GetPsicologoById(int id)
        {
            if (ObterUsuarioId() != id) return Forbid();

            var psicologo = await _psicologoService.GetPsicologoByIdAsync(id);
            return psicologo == null ? NotFound() : Ok(_mapper.Map<PsicologoDTO>(psicologo));
        }

        [Authorize(Roles = "Psicologo")]
        [HttpPatch("atualizar-psicologo/{id}")]
        public async Task<IActionResult> UpdatePsicologoParcial(int id, [FromBody] PsicologoUpdateDTO dto)
        {
            if (ObterUsuarioId() != id) return Forbid();

            var psicologo = await _psicologoService.GetPsicologoByIdAsync(id);
            if (psicologo == null) return NotFound("Psicólogo não encontrado.");

            // Aplicar atualizações
            if (!string.IsNullOrWhiteSpace(dto.Name)) psicologo.Name = dto.Name;
            if (!string.IsNullOrWhiteSpace(dto.Email)) psicologo.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.Password)) psicologo.Password = dto.Password;
            if (!string.IsNullOrWhiteSpace(dto.Phone)) psicologo.Phone = dto.Phone;
            if (!string.IsNullOrWhiteSpace(dto.CPF)) psicologo.CPF = dto.CPF;
            if (dto.DataNascimento.HasValue) psicologo.DataNascimento = dto.DataNascimento.Value;
            if (!string.IsNullOrWhiteSpace(dto.CRP)) psicologo.CRP = dto.CRP;

            await _psicologoService.UpdatePsicologoAsync(psicologo);
            return NoContent();
        }

        [Authorize(Roles = "Psicologo")]
        [HttpDelete("desativar-psicologo/{id}")]
        public async Task<IActionResult> DeletePsicologo(int id)
        {
            if (ObterUsuarioId() != id) return Forbid();

            var psicologo = await _psicologoService.GetPsicologoByIdAsync(id);
            if (psicologo == null) return NotFound();

            await _psicologoService.DesativarPsicologoAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Psicologo")]
        [HttpGet("minhas-criancas")]
        public async Task<IActionResult> GetMinhasCriancas()
        {
            var usuarioId = ObterUsuarioId();
            var criancas = await _context.Criancas
                .Where(c => c.PsicologoId == usuarioId)
                .ToListAsync();

            return Ok(criancas);
        }

        

        [Authorize(Roles = "Psicologo")]
        [HttpPost("criar-atividade")]
        public async Task<IActionResult> CriarAtividade([FromBody] AtividadeCreateDTO atividadeDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var psicologoId = ObterUsuarioId();
            var crianca = await _context.Criancas
                .FirstOrDefaultAsync(c => c.Id == atividadeDto.CriancaId && c.PsicologoId == psicologoId);

            if (crianca == null)
                return NotFound("Criança não encontrada ou não vinculada a este psicólogo.");

            var atividade = _mapper.Map<Atividade>(atividadeDto);
            atividade.PsicologoId = psicologoId;
            atividade.Concluida = false;
            atividade.DataCriacao = DateTime.UtcNow;

            await _atividadeService.AddAtividadeAsync(atividade);
            var novaAtividadeDto = _mapper.Map<AtividadeDTO>(atividade);

            return CreatedAtAction(nameof(GetMinhasCriancas), new { id = novaAtividadeDto.Id }, novaAtividadeDto);
        }

        [Authorize(Roles = "Psicologo")]
        [HttpGet("estatisticas")]
        public async Task<IActionResult> GerarEstatisticas()
        {
            var psicologoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var criancas = await _context.Criancas
                .Include(c => c.Atividades)
                .Where(c => c.PsicologoId == psicologoId)
                .ToListAsync();

            var estatisticas = criancas.Select(c => new
            {
                Crianca = c.Nome,
                AtividadesConcluidas = c.Atividades.Count(a => a.Concluida == true),
                AtividadesPendentes = c.Atividades.Count(a => a.Concluida == false)
            });

            return Ok(estatisticas);
        }

    }
}