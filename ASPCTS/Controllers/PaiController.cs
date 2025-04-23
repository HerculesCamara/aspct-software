using ASPCTS.DTOs;
using ASPCTS.Models;
using ASPCTS.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ASPCTS.DTOs.Pai;

namespace ASPCTS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaiController : ControllerBase
    {
        private readonly IPaiService _paiService;
        private readonly IMapper _mapper;

        public PaiController(IPaiService paiService, IMapper mapper)
        {
            _paiService = paiService;
            _mapper = mapper;
        }

        [HttpGet("BuscarTodosPais")]
        [ProducesResponseType(typeof(IEnumerable<PaiDTO>), 200)]
        public async Task<IActionResult> GetAllPais()
        {
            var pais = await _paiService.GetAllPaisAsync();
            var paisDto = _mapper.Map<IEnumerable<PaiDTO>>(pais);
            return Ok(paisDto);
        }

        [HttpGet("BuscarPorPaiId/{id}")]
        [ProducesResponseType(typeof(PaiDTO), 200)]
        public async Task<IActionResult> GetPaiById(int id)
        {
            var pai = await _paiService.GetPaiByIdAsync(id);
            if (pai == null)
                return NotFound();

            var paiDto = _mapper.Map<PaiDTO>(pai);
            return Ok(paiDto);
        }

        [HttpPost("AdicionarPai")]
        [ProducesResponseType(typeof(PaiDTO), 201)]
        public async Task<IActionResult> AddPai([FromBody] PaiCreateDTO novoPaiDto)
        {
            if (novoPaiDto == null)
                return BadRequest("Dados inválidos.");

            var existente = await _paiService.GetPaiByCPFAsync(novoPaiDto.CPF);
            if (existente != null)
                return Conflict("Já existe um pai cadastrado com esse CPF.");

            var pai = _mapper.Map<Pai>(novoPaiDto);
            pai.Tipo = "Pai";

            await _paiService.AddPaiAsync(pai);
            var paiCriado = _mapper.Map<PaiDTO>(pai);
            return CreatedAtAction(nameof(GetPaiById), new { id = pai.Id }, paiCriado);
        }

        [HttpPut("AtualizarPai/{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdatePai(int id, [FromBody] PaiUpdateDTO paiDto)
        {
            var paiExistente = await _paiService.GetPaiByIdAsync(id);
            if (paiExistente == null)
                return NotFound("Pai não encontrado.");

            _mapper.Map(paiDto, paiExistente);
            await _paiService.UpdatePaiAsync(paiExistente);
            return NoContent();
        }

        [HttpDelete("DeletarPai/{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeletePai(int id)
        {
            var pai = await _paiService.GetPaiByIdAsync(id);
            if (pai == null)
                return NotFound();

            await _paiService.DeletePaiAsync(id);
            return NoContent();
        }
    }
}
