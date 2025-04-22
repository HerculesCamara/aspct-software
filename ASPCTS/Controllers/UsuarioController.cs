using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;
using ASPCTS.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace ASPCTS.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        //GET: api/usuarios
        [HttpGet("BuscarTodosUsuarios")]
        [ProducesResponseType(typeof(IEnumerable<Usuario>), 200)]
        public async Task<IActionResult> GetAllUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.GetAllUsuariosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //GET: api/usuarios/{id}
        [HttpGet("BuscarPorUsuarioId/{id}")]
        [ProducesResponseType(typeof(Usuario), 200)]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
                if (usuario == null)
                {
                    return NotFound($"Usuario with id {id} not found.");
                }
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //POST: api/usuarios
        [HttpPost("AdicionarUsuario")]
        [ProducesResponseType(typeof(Usuario), 201)]
        public async Task<IActionResult> AddUsuario([FromBody] Usuario usuario)
        {
            try
            {
                if (usuario == null)
                {
                    return BadRequest("Usuario object is null");
                }
                await _usuarioService.AddUsuarioAsync(usuario);
                return CreatedAtAction(nameof(GetUsuarioById), new { id = usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // PUT: api/usuarios/{id}
        [HttpPut("AtualizarUsuario/{id}")]
        [ProducesResponseType(typeof(Usuario), 204)]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] Usuario usuarioAtualizado)
        {
            if (usuarioAtualizado == null || id != usuarioAtualizado.Id)
            {
                return BadRequest("Usuario não encontrado ou IDs incompatíveis.");
            }

            var existingUsuario = await _usuarioService.GetUsuarioByIdAsync(id);
            if (existingUsuario == null)
            {
                return NotFound("Usuario não encontrado.");
            }

            // Atualização de campos com base nos valores fornecidos
            existingUsuario.Name = !string.IsNullOrWhiteSpace(usuarioAtualizado.Name) ? usuarioAtualizado.Name : existingUsuario.Name;
            existingUsuario.CPF = !string.IsNullOrWhiteSpace(usuarioAtualizado.CPF) ? usuarioAtualizado.CPF : existingUsuario.CPF;
            existingUsuario.Phone = !string.IsNullOrWhiteSpace(usuarioAtualizado.Phone) ? usuarioAtualizado.Phone : existingUsuario.Phone;
            existingUsuario.Email = !string.IsNullOrWhiteSpace(usuarioAtualizado.Email) ? usuarioAtualizado.Email : existingUsuario.Email;
            existingUsuario.DataNascimento = usuarioAtualizado.DataNascimento != default(DateTimeOffset) ? usuarioAtualizado.DataNascimento : existingUsuario.DataNascimento;
            existingUsuario.Password = !string.IsNullOrWhiteSpace(usuarioAtualizado.Password) ? usuarioAtualizado.Password : existingUsuario.Password;

            // Atualizando o usuário no banco de dados
            await _usuarioService.UpdateUsuarioAsync(existingUsuario);

            return NoContent(); // Retorna 204 - No Content, indicando que a atualização foi bem-sucedida.
        }

        //DELETE: api/usuarios/{id}
        [HttpDelete("DeletarUsuario/{id}")]
        [ProducesResponseType(typeof(Usuario), 204)]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var existingUsuario = await _usuarioService.GetUsuarioByIdAsync(id);
                if (existingUsuario == null)
                {
                    return NotFound($"Usuario with id {id} not found.");
                }
                await _usuarioService.DeleteUsuarioAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}