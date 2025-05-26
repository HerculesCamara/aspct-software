using ASPCTS.DTOs;
using ASPCTS.Context;
using ASPCTS.Models;
using ASPCTS.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ASPCTS.DTOs.Responsavel;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace ASPCTS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsavelController : ControllerBase
    {
        private readonly IResponsavelService _responsavelService;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public ResponsavelController(IResponsavelService responsavelService, IMapper mapper, IJwtService jwtService)
        {
            _responsavelService = responsavelService;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        [Authorize(Roles = "Responsavel, Psicologo")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMeuPerfil()
        {
            var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var responsavel = await _responsavelService.GetResponsavelComCriancasAsync(id);

            if (responsavel == null) return NotFound("Responsável não encontrado.");

            var dto = _mapper.Map<ResponsavelDTO>(responsavel);
            return Ok(dto);
        }

        [Authorize(Roles = "Responsavel")]
        [HttpPatch("me")]
        public async Task<IActionResult> AtualizarMeuPerfil([FromBody] ResponsavelUpdateDTO dto)
        {
            var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var atualizado = await _responsavelService.AtualizarResponsavelAsync(id, dto);
            if (!atualizado) return NotFound("Responsável não encontrado.");
            return NoContent();
        }

        [Authorize(Roles = "Responsavel")]
        [HttpDelete("me")]
        public async Task<IActionResult> DesativarMeuCadastro()
        {
            var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var ok = await _responsavelService.DesativarResponsavelAsync(id);
            return ok ? NoContent() : NotFound("Responsável não encontrado.");
        }

        [Authorize(Roles = "Responsavel")]
        [HttpGet("criancas")]
        public async Task<IActionResult> MinhasCriancas()
        {
            var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var criancas = await _responsavelService.GetCriancasDoResponsavelAsync(id);
            return Ok(criancas);
        }

        [Authorize(Roles = "Responsavel")]
        [HttpGet("atividades")]
        public async Task<IActionResult> AtividadesCriancas()
        {
            var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var atividades = await _responsavelService.GetAtividadesDasCriancasAsync(id);
            return Ok(atividades);
        }

        [Authorize(Roles = "Responsavel")]
        [HttpGet("relatorios")]
        public async Task<IActionResult> RelatoriosCriancas()
        {
            var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var relatorios = await _responsavelService.GetRelatoriosDasCriancasAsync(id);
            return Ok(relatorios);
        }

        [Authorize(Roles = "Responsavel")]
        [HttpGet("psicologo")]
        public async Task<IActionResult> PsicologoVinculado()
        {
            var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var psicologo = await _responsavelService.GetPsicologoDasCriancasAsync(id);
            return psicologo == null ? NotFound("Nenhum psicólogo vinculado.") : Ok(psicologo);
        }

        [Authorize(Roles = "Psicologo")]
        [HttpGet("meus-responsaveis")]
        public async Task<IActionResult> GetResponsaveisVinculados()
        {
            var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var responsaveis = await _responsavelService.GetResponsaveisPorPsicologoAsync(id);
            return Ok(responsaveis);
        }

        [Authorize(Roles = "Psicologo")]
        [HttpPatch("editar-responsavel/{responsavelId}")]
        public async Task<IActionResult> AtualizarResponsavelComoPsicologo(int responsavelId, [FromBody] ResponsavelUpdateDTO dto)
        {
            var psicologoId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var atualizado = await _responsavelService.AtualizarResponsavelPorPsicologoAsync(psicologoId, responsavelId, dto);
            return atualizado ? NoContent() : Forbid("Acesso negado ou responsável não encontrado.");
        }
    }
}
