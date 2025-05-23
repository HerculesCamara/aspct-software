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
    public class responsavelController : ControllerBase
    {
        private readonly IResponsavelService _responsavelService;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly ApplicationDbContext _context;

        public responsavelController(IResponsavelService responsavelService, IMapper mapper, IJwtService jwtService, ApplicationDbContext context)
        {
            _responsavelService = responsavelService;
            _mapper = mapper;
            _jwtService = jwtService;
            _context = context;
        }

        [Authorize(Roles = "Responsavel")]
        [HttpGet("buscar-todos-responsaveis")]
        [ProducesResponseType(typeof(IEnumerable<ResponsavelDTO>), 200)]
        public async Task<IActionResult> GetAllPais()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var responsavel = await _responsavelService.GetResponsavelByIdAsync(usuarioId);
            if (responsavel == null) return Unauthorized();

            var responsaveisDto = new List<ResponsavelDTO> { _mapper.Map<ResponsavelDTO>(responsavel) };
            return Ok(responsaveisDto);
        }

        [Authorize(Roles = "Responsavel")]
        [HttpGet("buscar-responsavel-por-id/{id}")]
        [ProducesResponseType(typeof(ResponsavelDTO), 200)]
        public async Task<IActionResult> GetPaiById(int id)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            if (usuarioId != id) return Forbid();

            var responsavel = await _responsavelService.GetResponsavelByIdAsync(id);
            if (responsavel == null)
                return NotFound();

            var responsavelDTO = _mapper.Map<ResponsavelDTO>(responsavel);
            return Ok(responsavelDTO);
        }

        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> LoginResponsavel([FromBody] ResponsavelLoginDTO loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
                return BadRequest("Email e senha são obrigatórios.");

            var responsavel = await _responsavelService.GetResponsavelByEmailAsync(loginDto.Email);

            if (responsavel == null || responsavel.Password != loginDto.Password)
                return BadRequest("Email ou senha inválidos.");

            var token = _jwtService.GenerateToken(responsavel);

            return Ok(new
            {
                Token = token,
                Nome = responsavel.Name,
                Email = responsavel.Email,
                Tipo = responsavel.Tipo
            });
        }

        [HttpPost("adicionar-responsavel")]
        [ProducesResponseType(typeof(ResponsavelDTO), 201)]
        public async Task<IActionResult> AddPai([FromBody] ResponsavelCreateDTO novoResponsavelDTO)
        {
            if (novoResponsavelDTO == null)
                return BadRequest("Dados inválidos.");

            var existente = await _responsavelService.GetResponsaveisByCPFAsync(novoResponsavelDTO.CPF);
            if (existente != null)
                return Conflict("Já existe um responsável cadastrado com esse CPF.");

            var pai = _mapper.Map<Responsavel>(novoResponsavelDTO);
            pai.Tipo = "Pai"; // ou "Mãe", conforme sua lógica

            await _responsavelService.AddResponsavelAsync(pai);
            var paiCriado = _mapper.Map<ResponsavelDTO>(pai);
            return CreatedAtAction(nameof(GetPaiById), new { id = pai.Id }, paiCriado);
        }

        [Authorize(Roles = "Responsavel")]
        [HttpPatch("atualizar-responsavel/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateResponsavelParcial(int id, [FromBody] ResponsavelUpdateDTO responsavelDTO)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            if (usuarioId != id) return Forbid();

            var responsavelExistente = await _responsavelService.GetResponsavelByIdAsync(id);
            if (responsavelExistente == null)
                return NotFound("Responsável não encontrado.");

            if (!string.IsNullOrWhiteSpace(responsavelDTO.Name))
                responsavelExistente.Name = responsavelDTO.Name;

            if (!string.IsNullOrWhiteSpace(responsavelDTO.Email))
                responsavelExistente.Email = responsavelDTO.Email;

            if (!string.IsNullOrWhiteSpace(responsavelDTO.Password))
                responsavelExistente.Password = responsavelDTO.Password;

            if (!string.IsNullOrWhiteSpace(responsavelDTO.Phone))
                responsavelExistente.Phone = responsavelDTO.Phone;

            if (!string.IsNullOrWhiteSpace(responsavelDTO.CPF))
                responsavelExistente.CPF = responsavelDTO.CPF;

            if (responsavelDTO.DataNascimento.HasValue)
                responsavelExistente.DataNascimento = responsavelDTO.DataNascimento.Value;

            await _responsavelService.UpdateResponsavelAsync(responsavelExistente);

            return NoContent();
        }

        [Authorize(Roles = "Responsavel")]
        [HttpDelete("desativar-responsavel/{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeletePai(int id)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            if (usuarioId != id) return Forbid();

            var pai = await _responsavelService.GetResponsavelByIdAsync(id);
            if (pai == null)
                return NotFound();

            await _responsavelService.DesativarResponsavelAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Responsavel")]
        [HttpGet("minhas-criancas")]
        public async Task<IActionResult> GetMinhasCriancas()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var criancas = await _context.Criancas
                .Where(c => c.PaiId == usuarioId || c.MaeId == usuarioId)
                .ToListAsync();

            return Ok(criancas);
        }

        [Authorize(Roles = "Responsavel")]
        [HttpGet("atividades/minhas-criancas")]
        public async Task<IActionResult> GetAtividades()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var criancaIds = await _context.Criancas
                .Where(c => c.PaiId == usuarioId || c.MaeId == usuarioId)
                .Select(c => c.Id)
                .ToListAsync();

            var atividades = await _context.Atividades
                .Where(a => criancaIds.Contains(a.CriancaId))
                .ToListAsync();

            return Ok(atividades);
        }

        [Authorize(Roles = "Responsavel")]
        [HttpGet("relatorios/minhas-criancas")]
        public async Task<IActionResult> GetRelatorios()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var criancaIds = await _context.Criancas
                .Where(c => c.PaiId == usuarioId || c.MaeId == usuarioId)
                .Select(c => c.Id)
                .ToListAsync();

            var relatorios = await _context.Relatorios
                .Where(r => criancaIds.Contains(r.CriancaId))
                .ToListAsync();

            return Ok(relatorios);
        }

        [Authorize(Roles = "Responsavel")]
        [HttpGet("psicologo/minhas-criancas")]
        public async Task<IActionResult> GetPsicologo()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var psicologo = await _context.Criancas
                .Where(c => c.PaiId == usuarioId || c.MaeId == usuarioId)
                .Select(c => c.Psicologo)
                .FirstOrDefaultAsync();

            return psicologo == null ? NotFound("Nenhum psicólogo vinculado.") : Ok(psicologo);
        }
    }
}