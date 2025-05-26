using System.Collections.Generic;
using System.Threading.Tasks;
using ASPCTS.DTOs;
using ASPCTS.DTOs.Atividade;
using ASPCTS.Models;
using ASPCTS.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ASPCTS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class atividadeController : ControllerBase
    {
        private readonly IAtividadeService _atividadeService;
        private readonly ICriancaService _criancaService;
        private readonly IMapper _mapper;

        public atividadeController(IAtividadeService atividadeService, ICriancaService criancaService, IMapper mapper)
        {
            _atividadeService = atividadeService;
            _criancaService = criancaService;
            _mapper = mapper;
        }

        [HttpGet("buscar-atividades")]
        [Authorize(Roles = "Responsavel,Psicologo")]
        [ProducesResponseType(typeof(IEnumerable<AtividadeDTO>), 200)]
        public async Task<IActionResult> GetAllAtividades()
        {
            var atividades = await _atividadeService.GetAllAtividadesAsync();

            var atividadesFiltradas = new List<Atividade>();
            foreach (var atividade in atividades)
            {
                if (await _criancaService.UsuarioTemAcessoACriancaAsync(atividade.CriancaId, User))
                {
                    atividadesFiltradas.Add(atividade);
                }
            }

            var atividadesDto = _mapper.Map<IEnumerable<AtividadeDTO>>(atividadesFiltradas);
            return Ok(atividadesDto);
        }

        [HttpGet("buscar-atividade-id/{id}")]
        [Authorize(Roles = "Responsavel,Psicologo")]
        [ProducesResponseType(typeof(AtividadeDTO), 200)]
        public async Task<IActionResult> GetAtividadeById(int id)
        {
            var atividade = await _atividadeService.GetAtividadeByIdAsync(id);
            if (atividade == null) return NotFound();

            if (!await _criancaService.UsuarioTemAcessoACriancaAsync(atividade.CriancaId, User))
                return Forbid();

            var atividadeDto = _mapper.Map<AtividadeDTO>(atividade);
            return Ok(atividadeDto);
        }

        [HttpPost("adicionar-atividade")]
        [Authorize(Roles = "Psicologo")]
        [ProducesResponseType(typeof(AtividadeDTO), 400)]
        [ProducesResponseType(typeof(AtividadeDTO), 201)]
        public async Task<IActionResult> AddAtividade([FromBody] AtividadeCreateDTO atividadeDto)
        {
            if (atividadeDto == null)
                return BadRequest("Atividade não pode ser nula.");

            var atividade = _mapper.Map<Atividade>(atividadeDto);
            await _atividadeService.AddAtividadeAsync(atividade);
            var atividadeResultDto = _mapper.Map<AtividadeDTO>(atividade);
            return CreatedAtAction(nameof(GetAtividadeById), new { id = atividade.Id }, atividadeResultDto);
        }

        [HttpPatch("atualizar-atividade/{id}")]
        [Authorize(Roles = "Responsavel,Psicologo")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AtualizarAtividadeParcial(int id, [FromBody] AtividadeUpdateDTO dto)
        {
            var atividade = await _atividadeService.GetAtividadeByIdAsync(id);
            if (atividade == null) return NotFound();

            if (!await _criancaService.UsuarioTemAcessoACriancaAsync(atividade.CriancaId, User))
                return Forbid();

            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role == "Responsavel")
            {
                if (dto.Concluida.HasValue)
                {
                    atividade.Concluida = dto.Concluida.Value;
                    atividade.Ativo = dto.Concluida.Value;
                    atividade.DataConclusao = dto.Concluida.Value ? DateTime.UtcNow : null;
                }
            }
            else if (role == "Psicologo")
            {
                if (!string.IsNullOrEmpty(dto.Titulo))
                    atividade.Titulo = dto.Titulo;
                if (!string.IsNullOrEmpty(dto.Descricao))
                    atividade.Descricao = dto.Descricao;
                if (dto.Concluida.HasValue)
                {
                    atividade.Concluida = dto.Concluida.Value;
                    atividade.Ativo = dto.Concluida.Value;
                    atividade.DataConclusao = dto.Concluida.Value ? DateTime.UtcNow : null;
                }
                if (dto.DataConclusao.HasValue)
                    atividade.DataConclusao = dto.DataConclusao;
            }

            await _atividadeService.UpdateAtividadeAsync(atividade);
            return NoContent();
        }

        [HttpDelete("desativar-atividade/{id}")]
        [Authorize(Roles = "Psicologo")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DesativarAtividade(int id)
        {
            var atividade = await _atividadeService.GetAtividadeByIdAsync(id);
            if (atividade == null) return NotFound();

            if (!await _criancaService.UsuarioTemAcessoACriancaAsync(atividade.CriancaId, User))
                return Forbid();

            await _atividadeService.DesativarAtividadeAsync(id);
            return NoContent();
        }
    }
}
