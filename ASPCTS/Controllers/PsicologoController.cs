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
using ASPCTS.DTOs.Relatorio;

namespace ASPCTS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class psicologoController : ControllerBase
    {
        private readonly IPsicologoService _psicologoService;
        private readonly IAtividadeService _atividadeService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public psicologoController(IPsicologoService psicologoService, IAtividadeService atividadeService, IMapper mapper, ApplicationDbContext context)
        {
            _psicologoService = psicologoService;
            _atividadeService = atividadeService;
            _mapper = mapper;
            _context = context;
        }

        [Authorize(Roles = "Psicologo")]
        [HttpGet("buscar-todos-psicologos")]
        [ProducesResponseType(typeof(IEnumerable<PsicologoDTO>), 200)]
        public async Task<IActionResult> GetAllPsicologos()
        {
            var psicologosDto = await _psicologoService.GetAllPsicologosAsync();
            return Ok(psicologosDto);
        }

        [Authorize(Roles = "Psicologo")]
        [HttpGet("buscar-psicologo-por-id/{id}")]
        [ProducesResponseType(typeof(PsicologoDTO), 200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPsicologoById(int id)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            if (usuarioId != id) return Forbid();

            var psicologo = await _psicologoService.GetPsicologoByIdAsync(id);
            if (psicologo == null)
            {
                return NotFound();
            }

            var psicologoDto = _mapper.Map<PsicologoDTO>(psicologo);
            return Ok(psicologoDto);
        }

        [Authorize(Roles = "Psicologo")]
        [HttpPatch("atualizar-psicologo/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePsicologoParcial(int id, [FromBody] PsicologoUpdateDTO psicologoDto)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            if (usuarioId != id) return Forbid();

            var psicologoExistente = await _psicologoService.GetPsicologoByIdAsync(id);
            if (psicologoExistente == null)
            {
                return NotFound("Psicólogo não encontrado.");
            }

            // Atualiza somente os campos enviados no DTO
            if (!string.IsNullOrWhiteSpace(psicologoDto.Name))
                psicologoExistente.Name = psicologoDto.Name;

            if (!string.IsNullOrWhiteSpace(psicologoDto.Email))
                psicologoExistente.Email = psicologoDto.Email;

            if (!string.IsNullOrWhiteSpace(psicologoDto.Password))
                psicologoExistente.Password = psicologoDto.Password;

            if (!string.IsNullOrWhiteSpace(psicologoDto.Phone))
                psicologoExistente.Phone = psicologoDto.Phone;

            if (!string.IsNullOrWhiteSpace(psicologoDto.CPF))
                psicologoExistente.CPF = psicologoDto.CPF;

            if (psicologoDto.DataNascimento.HasValue)
                psicologoExistente.DataNascimento = psicologoDto.DataNascimento.Value;

            if (!string.IsNullOrWhiteSpace(psicologoDto.CRP))
                psicologoExistente.CRP = psicologoDto.CRP;

            await _psicologoService.UpdatePsicologoAsync(psicologoExistente);

            return NoContent();
        }

        [Authorize(Roles = "Psicologo")]
        [HttpDelete("desativar-psicologo/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePsicologo(int id)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            if (usuarioId != id) return Forbid();

            var psicologo = await _psicologoService.GetPsicologoByIdAsync(id);
            if (psicologo == null)
            {
                return NotFound();
            }

            await _psicologoService.DesativarPsicologoAsync(id);
            return NoContent();
        }

        // Exemplo de endpoint adicional com filtro baseado no psicologo logado
        [Authorize(Roles = "Psicologo")]
        [HttpGet("minhas-criancas")]
        public async Task<IActionResult> GetMinhasCriancas()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var criancas = await _context.Criancas
                .Where(c => c.PsicologoId == usuarioId)
                .ToListAsync();

            return Ok(criancas);
        }
    }
}