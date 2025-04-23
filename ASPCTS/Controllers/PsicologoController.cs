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
    public class PsicologoController : ControllerBase
    {
        private readonly IPsicologoService _psicologoService;
        private readonly IMapper _mapper;
        public PsicologoController(IPsicologoService psicologoService, IMapper mapper)
        {
            _psicologoService = psicologoService;
            _mapper = mapper;
        }

        //GET: api/psicologo
        [HttpGet("BuscarTodosPsicologos")]
        [ProducesResponseType(typeof(IEnumerable<PsicologoDTO>), 200)]
        public async Task<IActionResult> GetAllPsicologos()
        {
            var psicologosDto = await _psicologoService.GetAllPsicologosAsync();
            return Ok(psicologosDto);
        }

        //GET: api/psicologo/{id}
        [HttpGet("BuscarPorPsicologoId/{id}")]
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
        [HttpPost("AdicionarPsicologo")]
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
        [HttpPut("AtualizarPsicologo/{id}")]
        [ProducesResponseType(typeof(Psicologo), 204)]
        public async Task<IActionResult> UpdatePsicologo(int id, PsicologoUpdateDTO psicologoAtualizado)
        {
            var psicologoExistente = await _psicologoService.GetPsicologoByIdAsync(id);
            if (psicologoExistente == null)
            {
                return NotFound("Psicólogo não encontrado.");
            }

            _mapper.Map(psicologoAtualizado, psicologoExistente); // Mapeia os dados atualizados para o psicólogo existente
            await _psicologoService.UpdatePsicologoAsync(psicologoExistente); // Atualiza o psicólogo no banco de dados

            return NoContent(); // Retorna 204 - No Content, indicando que a atualização foi bem-sucedida.
        }


        //DELETE: api/psicologo/{id}
        [HttpDelete("DeletarPsicologo/{id}")]
        [ProducesResponseType(typeof(Psicologo), 204)]
        public async Task<IActionResult> DeletePsicologo(int id)
        {
            var psicologo = await _psicologoService.GetPsicologoByIdAsync(id);
            if (psicologo == null)
            {
                return NotFound();
            }
            await _psicologoService.DeletePsicologoAsync(id);
            return NoContent();
        }
    }
}