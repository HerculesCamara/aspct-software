using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;
using ASPCTS.Repositories;

namespace ASPCTS.Services
{
    public class PaiService : IPaiService
    {
        private readonly IPaiRepository _paiRepository;

        public PaiService(IPaiRepository paiRepository)
        {
            _paiRepository = paiRepository;
        }

        public async Task<IEnumerable<Pai>> GetAllPaisAsync()
        {
            return await _paiRepository.GetAllPaisAsync();
        }

        public async Task<Pai?> GetPaiByIdAsync(int id)
        {
            return await _paiRepository.GetPaiByIdAsync(id);
        }

        public async Task<IEnumerable<Pai>> GetPaiByNameAsync(string name)
        {
            return await _paiRepository.GetPaiByNameAsync(name);
        }

        public async Task<Pai?> GetPaiByCPFAsync(string cpf)
        {
            return await _paiRepository.GetPaiByCPFAsync(cpf);
        }

        public async Task AddPaiAsync(Pai pai)
        {
            var pais = await _paiRepository.GetAllPaisAsync();

            //Validacao de CPF para evitar duplicidade de Pai
            if (pais.Any(u => u.CPF == pai.CPF))
            {
                throw new Exception("CPF já cadastrado.");
            }

            //Validacao de email para evitar duplicidade de email 
            if (pais.Any(u => u.Email == pai.Email))
            {
                throw new Exception("Email já cadastrado.");
            }

            //Mensagens de erro para os campos obrigatorios
            var mensagensErro = new List<string>();

            if (string.IsNullOrWhiteSpace(pai.Name))
                mensagensErro.Add("O campo 'Nome' é obrigatório.");
            if (string.IsNullOrWhiteSpace(pai.CPF))
                mensagensErro.Add("O campo 'CPF' é obrigatório.");

            await _paiRepository.AddPaiAsync(pai);
        }

        public async Task UpdatePaiAsync(Pai pai)
        {
            var pais = await _paiRepository.GetAllPaisAsync();
            //Pesquisa do pai por parametros
            var existing = pais.FirstOrDefault(u =>
            u.Id == pai.Id ||
            u.CPF == pai.CPF ||
            u.Email.Equals(pai.Email, StringComparison.OrdinalIgnoreCase) ||
            (!string.IsNullOrWhiteSpace(pai.Name) && u.Name.Equals(pai.Name, StringComparison.OrdinalIgnoreCase)));

            if (existing == null)
            {
                throw new Exception("Pai não encontrado.");
            }

            await _paiRepository.UpdatePaiAsync(pai);
        }
        
        public async Task DesativarPaiAsync(int id)
        {
            var pais = await _paiRepository.GetAllPaisAsync();
            var pai = pais.FirstOrDefault(u => u.Id == id);

            if (pai == null)
            {
                throw new Exception("Pai não encontrado.");
            }
            if (!pai.Ativo)
            {
                throw new Exception("Pai já está desativado.");
            }

            await _paiRepository.DesativarPaiAsync(id);
        }
    }
}