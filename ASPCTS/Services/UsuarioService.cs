using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;
using ASPCTS.Repositories;

namespace ASPCTS.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
        {
            return await _usuarioRepository.GetAllUsuariosAsync();
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            return await _usuarioRepository.GetUsuarioByIdAsync(id);
        }

        public async Task<IEnumerable<Usuario>> GetUsuarioByNameAsync(string name)
        {
            return await _usuarioRepository.GetUsuarioByNameAsync(name);
        }

        public async Task<Usuario?> GetUsuarioByCPFAsync(string cpf)
        {
            return await _usuarioRepository.GetUsuarioByCPFAsync(cpf);
        }

        public async Task AddUsuarioAsync(Usuario usuario)
        {
            var usuarios = await _usuarioRepository.GetAllUsuariosAsync();
            //Validacao de CPF para evitar duplicidade de Usuario
            if (usuarios.Any(u => u.CPF == usuario.CPF))
            {
                throw new Exception("CPF já cadastrado.");
            }
            //Validacao de email para evitar duplicidade de email
            if (usuarios.Any(u => u.Email == usuario.Email))
            {
                throw new Exception("Email já cadastrado.");
            }

            //Mensagens de erro para os campos obrigatorios

            var mensagensErro = new List<string>();

            if (string.IsNullOrWhiteSpace(usuario.Name))
                mensagensErro.Add("O campo 'Nome' é obrigatório.");
            if (string.IsNullOrWhiteSpace(usuario.CPF))
                mensagensErro.Add("O campo 'CPF' é obrigatório.");
            if (string.IsNullOrWhiteSpace(usuario.Email))
                mensagensErro.Add("O campo 'Email' é obrigatório.");

            if (string.IsNullOrWhiteSpace(usuario.Password))
                mensagensErro.Add("O campo 'Senha' é obrigatório.");

            if (string.IsNullOrWhiteSpace(usuario.Phone))
                mensagensErro.Add("O campo 'Telefone' é obrigatório.");

            if (string.IsNullOrWhiteSpace(usuario.Tipo))
                mensagensErro.Add("O campo 'Tipo' é obrigatório.");

            if (mensagensErro.Any())
                throw new Exception(string.Join(" ", mensagensErro));

            await _usuarioRepository.AddUsuarioAsync(usuario);
        }

        public async Task UpdateUsuarioAsync(Usuario usuario)
        {
            var usuarios = await _usuarioRepository.GetAllUsuariosAsync();
            //Pesquisa do pai por parametros
            var existing = usuarios.FirstOrDefault(u =>
            u.Id == usuario.Id ||
            u.CPF == usuario.CPF ||
            u.Email.Equals(usuario.Email, StringComparison.OrdinalIgnoreCase)||
            (!string.IsNullOrWhiteSpace(usuario.Name) && u.Name.Equals(usuario.Name, StringComparison.OrdinalIgnoreCase)));
            
            if (existing == null)
            {
                throw new Exception("Pai não encontrado.");
            }

            await _usuarioRepository.UpdateUsuarioAsync(usuario);
        }

        public async Task DeleteUsuarioAsync(int id)
        {
            var existingUsuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
            if (existingUsuario == null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            await _usuarioRepository.DeleteUsuarioAsync(id);
        }
    }
}