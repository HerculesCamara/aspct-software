using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.DTOs.Psicologo;
using ASPCTS.Models;
using ASPCTS.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASPCTS.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class psicologoController : ControllerBase
    {
        private readonly IPsicologoService _psicologoService;
        private readonly IMapper _mapper;
        public psicologoController(IPsicologoService psicologoService, IMapper mapper)
        {
            _psicologoService = psicologoService;
            _mapper = mapper;
        }

        //GET: api/psicologo
        [HttpGet("buscar-todos-psicologos")]
        [ProducesResponseType(typeof(IEnumerable<PsicologoDTO>), 200)]
        public async Task<IActionResult> GetAllPsicologos()
        {
            var psicologosDto = await _psicologoService.GetAllPsicologosAsync();
            return Ok(psicologosDto);
        }

        //GET: api/psicologo/{id}
        [HttpGet("buscar-psicologo-por-id/{id}")]
        [ProducesResponseType(typeof(PsicologoDTO), 200)]
        public async Task<IActionResult> GetPsicologoById(int id)
        {
            var psicologo = await _psicologoService.GetPsicologoByIdAsync(id);
            if (psicologo == null)
            {
                return NotFound();
            }
            var psicologoDto = _mapper.Map<PsicologoDTO>(psicologo);
            return Ok(psicologoDto);
        }

        //POST: api/psicologo
        [HttpPost("adicionar-psicologo")]
        [ProducesResponseType(typeof(Psicologo), 201)]
        public async Task<IActionResult> AddPsicologo(PsicologoCreateDTO novoPsicologoDto)
        {
            if (novoPsicologoDto == null)
            {
                return BadRequest("Dados inválidos.");
            }

            var psicologo = _mapper.Map<Psicologo>(novoPsicologoDto);
            psicologo.Tipo = "Psicologo";

            var existingPsicologo = await _psicologoService.GetPsicologoByCPFAsync(psicologo.CPF);
            if (existingPsicologo != null)
            {
                return Conflict("Já existe um psicólogo cadastrado com esse CPF.");
            }

            await _psicologoService.AddPsicologoAsync(psicologo);
            var novoPsicologo = _mapper.Map<PsicologoDTO>(psicologo);
            return CreatedAtAction(nameof(GetPsicologoById), new { id = psicologo.Id }, novoPsicologo);
        }

        // PUT: api/psicologo/{id}
        [HttpPatch("atualizar-psicologo/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePsicologoParcial(int id, [FromBody] PsicologoUpdateDTO psicologoDto)
        {
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



        //DELETE: api/psicologo/{id}
        [HttpDelete("desativar-psicologo/{id}")]
        [ProducesResponseType(typeof(Psicologo), 204)]
        public async Task<IActionResult> DeletePsicologo(int id)
        {
            var psicologo = await _psicologoService.GetPsicologoByIdAsync(id);
            if (psicologo == null)
            {
                return NotFound();
            }
            await _psicologoService.DesativarPsicologoAsync(id);
            return NoContent();
        }
    }
}