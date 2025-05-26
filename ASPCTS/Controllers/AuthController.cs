using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Context;
using ASPCTS.DTOs.Login;
using ASPCTS.DTOs.Register;
using ASPCTS.Models;
using ASPCTS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPCTS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class authController : ControllerBase
    {
        private readonly IResponsavelService _responsavelService;
        private readonly IPsicologoService _psicologoService;
        private readonly IJwtService _jwtService;
        private readonly ApplicationDbContext _context;

        public authController(IResponsavelService responsavelService, IPsicologoService psicologoService, IJwtService jwtService, ApplicationDbContext context)
        {
            _responsavelService = responsavelService;
            _psicologoService = psicologoService;
            _jwtService = jwtService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Invalid login request.");
            }

            object? usuario = await _responsavelService.GetResponsavelByEmailAsync(loginDto.Email);
            if (usuario == null)
            {
                usuario = await _psicologoService.GetPsicologoByEmailAsync(loginDto.Email);
            }

            bool isValid = false;
            if (usuario is ASPCTS.Models.Responsavel responsavel)
            {
                isValid = responsavel.VerifyPassword(loginDto.Password);
            }
            else if (usuario is ASPCTS.Models.Psicologo psicologo)
            {
                isValid = psicologo.VerifyPassword(loginDto.Password);
            }

            if (usuario == null || !isValid)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = await _jwtService.GenerateToken((ASPCTS.Models.Usuario)usuario);
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioRegisterDTO dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            {
                return Conflict("Já existe um usuário com esse e-mail.");
            }

            Usuario novoUsuario;
            if (dto.Tipo.ToLower() == "psicologo")
            {
                var psicologo = new Psicologo
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    CPF = dto.CPF,
                    Tipo = "Psicologo",
                    Sexo = dto.Sexo,
                    DataNascimento = dto.DataNascimento,
                    CRP = dto.CRP ?? string.Empty
                };
                psicologo.SetPassword(dto.Password);
                novoUsuario = psicologo;
            }
            else if (dto.Tipo.ToLower() == "responsavel")
            {
                var responsavel = new Responsavel
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    CPF = dto.CPF,
                    Tipo = "Responsavel",
                    Sexo = dto.Sexo,
                    DataNascimento = dto.DataNascimento,
                    PsicologoId = dto.PsicologoId
                };
                responsavel.SetPassword(dto.Password);
                novoUsuario = responsavel;
            }
            else
            {
                return BadRequest("Tipo de usuário inválido. Use 'Psicologo' ou 'Responsavel'.");
            }

            _context.Add(novoUsuario);
            await _context.SaveChangesAsync();

            return Created("", new { novoUsuario.Id, novoUsuario.Name, novoUsuario.Email, novoUsuario.Tipo });
        }
    }
}