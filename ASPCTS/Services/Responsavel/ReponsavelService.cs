using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;
using ASPCTS.Repositories;

namespace ASPCTS.Services
{
    public class ResponsavelService : IResponsavelService
    {
        private readonly IResponsavelRepository _responsavelRepository;

        public ResponsavelService(IResponsavelRepository ResponsavelRepository)
        {
            _responsavelRepository = ResponsavelRepository;
        }

        public async Task<IEnumerable<Responsavel>> GetAllResponsaveisAsync()
        {
            return await _responsavelRepository.GetAllResponsaveisAsync();
        }
        public async Task<IEnumerable<Responsavel>> GetAllPaisAsync()
        {
            var todos = await _responsavelRepository.GetAllResponsaveisAsync();
            return todos.Where(p => p.Sexo == Usuario.Genero.Masculino);
        }

        public async Task<IEnumerable<Responsavel>> GetAllMaesAsync()
        {
            var todos = await _responsavelRepository.GetAllResponsaveisAsync();
            return todos.Where(p => p.Sexo == Usuario.Genero.Feminino);
        }


        public async Task<Responsavel?> GetResponsavelByIdAsync(int id)
        {
            return await _responsavelRepository.GetResponsavelByIdAsync(id);
        }
        public async Task<Responsavel?> GetResponsavelByCPFAsync(string cpf)
        {
            var responsaveis = await _responsavelRepository.GetAllResponsaveisAsync();
            return responsaveis.FirstOrDefault(u => u.CPF == cpf);
        }
        public async Task AddResponsavelAsync(Responsavel responsavel)
        {
            var responsaveis = await _responsavelRepository.GetAllResponsaveisAsync();

            //Validacao de CPF para evitar duplicidade de Responsavel
            if (responsaveis.Any(u => u.CPF == responsavel.CPF))
            {
                throw new Exception("CPF já cadastrado.");
            }

            //Validacao de email para evitar duplicidade de email 
            if (responsaveis.Any(u => u.Email == responsavel.Email))
            {
                throw new Exception("Email já cadastrado.");
            }

            //Mensagens de erro para os campos obrigatorios
            var mensagensErro = new List<string>();

            if (string.IsNullOrWhiteSpace(responsavel.Name))
                mensagensErro.Add("O campo 'Nome' é obrigatório.");
            if (string.IsNullOrWhiteSpace(responsavel.CPF))
                mensagensErro.Add("O campo 'CPF' é obrigatório.");

            await _responsavelRepository.AddResponsavelAsync(responsavel);
        }

        public async Task UpdateResponsavelAsync(Responsavel responsavel)
        {
            var responsaveis = await _responsavelRepository.GetAllResponsaveisAsync();
            //Pesquisa do Responsavel por parametros
            var existing = responsaveis.FirstOrDefault(u =>
            u.Id == responsavel.Id ||
            u.CPF == responsavel.CPF ||
            u.Email.Equals(responsavel.Email, StringComparison.OrdinalIgnoreCase) ||
            (!string.IsNullOrWhiteSpace(responsavel.Name) && u.Name.Equals(responsavel.Name, StringComparison.OrdinalIgnoreCase)));

            if (existing == null)
            {
                throw new Exception("Responsavel não encontrado.");
            }

            await _responsavelRepository.UpdateResponsavelAsync(responsavel);
        }

        public async Task DesativarResponsavelAsync(int id)
        {
            var responsaveis = await _responsavelRepository.GetAllResponsaveisAsync();
            var responsavel = responsaveis.FirstOrDefault(u => u.Id == id);

            if (responsavel == null)
            {
                throw new Exception("Responsavel não encontrado.");
            }
            if (!responsavel.Ativo)
            {
                throw new Exception("Responsavel já está desativado.");
            }

            await _responsavelRepository.DesativarResponsavelAsync(id);
        }
    }
}