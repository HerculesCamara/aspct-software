using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;
using ASPCTS.Repositories;

namespace ASPCTS.Services
{
    public class CriancaService : ICriancaService
    {
        private readonly ICriancaRepository _criancaRepository;

        public CriancaService(ICriancaRepository criancaRepository)
        {
            _criancaRepository = criancaRepository;
        }

        public async Task<IEnumerable<Crianca>> GetAllCriancasAsync()
        {
            return await _criancaRepository.GetAllCriancasAsync();
        }

        public async Task<Crianca?> GetCriancaByIdAsync(int id)
        {
            return await _criancaRepository.GetCriancaByIdAsync(id);
        }

        public async Task<IEnumerable<Crianca>> GetCriancaByNameAsync(string name)
        {
            return await _criancaRepository.GetCriancaByNameAsync(name);
        }

        public async Task<Crianca?> GetCriancaByPaiIdAsync(int idPai)
        {
            return await _criancaRepository.GetCriancaByPaiIdAsync(idPai);
        }

        public async Task AddCriancaAsync(Crianca crianca)
        {
            var criancas = await _criancaRepository.GetAllCriancasAsync();



            //Mensagens de erro para os campos obrigatorios
            var mensagensErro = new List<string>();

            if (string.IsNullOrWhiteSpace(crianca.Nome))
                mensagensErro.Add("O campo 'Nome' é obrigatório.");
            if (string.IsNullOrWhiteSpace(crianca.Idade.ToString()))
                mensagensErro.Add("O campo 'Idade' é obrigatório.");

            if (mensagensErro.Any())
                throw new Exception(string.Join(", ", mensagensErro));

            await _criancaRepository.AddCriancaAsync(crianca);
        }

        public async Task UpdateCriancaAsync(Crianca crianca)
        {
            var criancas = await _criancaRepository.GetAllCriancasAsync();

            //Mensagens de erro para os campos obrigatorios
            var mensagensErro = new List<string>();

            if (string.IsNullOrWhiteSpace(crianca.Nome))
                mensagensErro.Add("O campo 'Nome' é obrigatório.");
            if (mensagensErro.Any())
                throw new Exception(string.Join(", ", mensagensErro));

            await _criancaRepository.UpdateCriancaAsync(crianca);
        }
        
        public async Task DesativarCriancaAsync(int id)
        {
            var criancas = await _criancaRepository.GetAllCriancasAsync();

            //Verifica se a criança existe
            if (!criancas.Any(u => u.Id == id))
            {
                throw new Exception("Criança não encontrada.");
            }
            //Verifica se a criança já está desativada
            var crianca = await _criancaRepository.GetCriancaByIdAsync(id);
            if (crianca != null && !crianca.Ativo)
            {
                throw new Exception("Criança já está desativada.");
            }

            await _criancaRepository.DesativarCriancaAsync(id);
        }
    }
}