using ASPCTS.DTOs;
using ASPCTS.Models;
using ASPCTS.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ASPCTS.DTOs.Responsavel;

namespace ASPCTS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class responsavelController : ControllerBase
    {
        private readonly IResponsavelService _responsavelService;
        private readonly IMapper _mapper;

        public responsavelController(IResponsavelService responsavelService, IMapper mapper)
        {
            _responsavelService = responsavelService;
            _mapper = mapper;
        }

        [HttpGet("buscar-todos-responsaveis")]

        [ProducesResponseType(typeof(IEnumerable<ResponsavelDTO>), 200)]
        public async Task<IActionResult> GetAllPais()
        {
            var responsaveis = await _responsavelService.GetAllPaisAsync();
            var responsaveisDto = _mapper.Map<IEnumerable<ResponsavelDTO>>(responsaveis);
            return Ok(responsaveisDto);
        }

        [HttpGet("buscar-responsavel-por-id/{id}")]
        [ProducesResponseType(typeof(ResponsavelDTO), 200)]
        public async Task<IActionResult> GetPaiById(int id)
        {
            var responsavel = await _responsavelService.GetResponsavelByIdAsync(id);
            if (responsavel == null)
                return NotFound();

            var responsavelDTO = _mapper.Map<ResponsavelDTO>(responsavel);
            return Ok(responsavelDTO);
        }

        [HttpPost("adicionar-responsavel")]
        [ProducesResponseType(typeof(ResponsavelDTO), 201)]
        public async Task<IActionResult> AddPai([FromBody] ResponsavelCreateDTO novoResponsavelDTO)
        {
            if (novoResponsavelDTO == null)
                return BadRequest("Dados inválidos.");

            var existente = await _responsavelService.GetResponsavelByCPFAsync(novoResponsavelDTO.CPF);
            if (existente != null)
                return Conflict("Já existe um pai cadastrado com esse CPF.");

            var pai = _mapper.Map<Responsavel>(novoResponsavelDTO);
            pai.Tipo = "Pai";

            await _responsavelService.AddResponsavelAsync(pai);
            var paiCriado = _mapper.Map<ResponsavelDTO>(pai);
            return CreatedAtAction(nameof(GetPaiById), new { id = pai.Id }, paiCriado);
        }

        [HttpPatch("atualizar-responsavel/{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateResponsavelParcial(int id, [FromBody] ResponsavelUpdateDTO responsavelDTO)
        {
            var responsavelExistente = await _responsavelService.GetResponsavelByIdAsync(id);
            if (responsavelExistente == null)
                return NotFound("Responsavel não encontrado.");

            // Atualiza apenas os campos fornecidos no DTO
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


        [HttpDelete("desativar-responsavel/{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeletePai(int id)
        {
            var pai = await _responsavelService.GetResponsavelByIdAsync(id);
            if (pai == null)
                return NotFound();

            await _responsavelService.DesativarResponsavelAsync(id);
            return NoContent();
        }
    }
}
