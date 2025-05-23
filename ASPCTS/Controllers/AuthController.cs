using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.DTOs.Login;
using ASPCTS.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASPCTS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class authController : ControllerBase
    {
        private readonly IResponsavelService _responsavelService;
        private readonly IPsicologoService _psicologoService;
        private readonly IJwtService _jwtService;

        public authController(IResponsavelService responsavelService, IPsicologoService psicologoService, IJwtService jwtService)
        {
            _responsavelService = responsavelService;
            _psicologoService = psicologoService;
            _jwtService = jwtService;
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
    }
}