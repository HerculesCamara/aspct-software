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

        private int UsuarioId
        {
            get
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim))
                    throw new UnauthorizedAccessException("Usuário não autenticado.");
                return int.Parse(userIdClaim);
            }
        }

        public ResponsavelController(IResponsavelService responsavelService, IMapper mapper, IJwtService jwtService)
        {
            _responsavelService = responsavelService;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        [Authorize(Roles = "Responsavel")]
        [HttpGet("buscar-perfil-responsavel")]
        [ProducesResponseType(typeof(ResponsavelDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetMeuPerfil()
        {
            var responsavel = await _responsavelService.GetResponsavelComCriancasAsync(UsuarioId);

            if (responsavel == null)
                return NotFound(new { mensagem = "Responsável não encontrado." });

            var dto = _mapper.Map<ResponsavelDTO>(responsavel);
            return Ok(dto);
        }

        [Authorize(Roles = "Responsavel")]
        [HttpPatch("atualizar-meu-perfil")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AtualizarMeuPerfil([FromBody] ResponsavelUpdateDTO dto)
        {
            var atualizado = await _responsavelService.AtualizarResponsavelAsync(UsuarioId, dto);
            if (!atualizado)
                return NotFound(new { mensagem = "Responsável não encontrado." });

            return NoContent();
        }

        [Authorize(Roles = "Responsavel")]
        [HttpDelete("desativar-meu-cadastro")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DesativarMeuCadastro()
        {
            var ok = await _responsavelService.DesativarResponsavelAsync(UsuarioId);
            return ok ? NoContent() : NotFound(new { mensagem = "Responsável não encontrado." });
        }

        [Authorize(Roles = "Responsavel")]
        [HttpGet("criancas")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> MinhasCriancas()
        {
            var criancas = await _responsavelService.GetCriancasDoResponsavelAsync(UsuarioId);
            return Ok(criancas);
        }

        [Authorize(Roles = "Responsavel")]
        [HttpGet("atividades")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AtividadesCriancas()
        {
            var atividades = await _responsavelService.GetAtividadesDasCriancasAsync(UsuarioId);
            return Ok(atividades);
        }

        [Authorize(Roles = "Responsavel")]
        [HttpGet("relatorios")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RelatoriosCriancas()
        {
            var relatorios = await _responsavelService.GetRelatoriosDasCriancasAsync(UsuarioId);
            return Ok(relatorios);
        }

        [Authorize(Roles = "Responsavel")]
        [HttpGet("psicologo")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PsicologoVinculado()
        {
            var psicologo = await _responsavelService.GetPsicologoDasCriancasAsync(UsuarioId);
            return psicologo == null
                ? NotFound(new { mensagem = "Nenhum psicólogo vinculado." })
                : Ok(psicologo);
        }

        [Authorize(Roles = "Psicologo")]
        [HttpGet("meus-responsaveis")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetResponsaveisVinculados()
        {
            var responsaveis = await _responsavelService.GetResponsaveisPorPsicologoAsync(UsuarioId);
            return Ok(responsaveis);
        }

        [Authorize(Roles = "Psicologo")]
        [HttpPatch("responsaveis/{responsavelId}/editar")]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AtualizarResponsavelComoPsicologo(int responsavelId, [FromBody] ResponsavelUpdateDTO dto)
        {
            if (dto == null)
                return BadRequest(new { mensagem = "Dados inválidos para atualização." });

            var atualizado = await _responsavelService.AtualizarResponsavelPorPsicologoAsync(UsuarioId, responsavelId, dto);
            return atualizado
                ? NoContent()
                : Forbid("Acesso negado ou responsável não encontrado.");
        }
    }
}
