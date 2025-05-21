using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;
using ASPCTS.Repositories;

namespace ASPCTS.Services
{
    public class PsicologoService : IPsicologoService
    {
        private readonly IPsicologoRepository _psicologoRepository;

        public PsicologoService(IPsicologoRepository psicologoRepository)
        {
            _psicologoRepository = psicologoRepository;
        }

        public async Task<IEnumerable<Psicologo>> GetAllPsicologosAsync()
        {
            return await _psicologoRepository.GetAllPsicologosAsync();
        }

        public async Task<Psicologo?> GetPsicologoByIdAsync(int id)
        {
            return await _psicologoRepository.GetPsicologoByIdAsync(id);
        }

        public async Task<IEnumerable<Psicologo>> GetPsicologoByNameAsync(string name)
        {
            return await _psicologoRepository.GetPsicologoByNameAsync(name);
        }

        public async Task<Psicologo?> GetPsicologoByCPFAsync(string cpf)
        {
            return await _psicologoRepository.GetPsicologoByCPFAsync(cpf);
        }

        public async Task<Psicologo?> GetPsicologoByCrpAsync(string crp)
        {
            return await _psicologoRepository.GetPsicologoByCrpAsync(crp);
        }

        public async Task AddPsicologoAsync(Psicologo psicologo)
        {
            var psicologos = await _psicologoRepository.GetAllPsicologosAsync();
            //Validacao de CPF para evitar duplicidade de Psicologo
            if (psicologos.Any(u => u.CPF == psicologo.CPF))
            {
                throw new Exception("CPF já cadastrado.");
            }
            //Validacao de email para evitar duplicidade de email
            if (psicologos.Any(u => u.Email == psicologo.Email))
            {
                throw new Exception("Email já cadastrado.");
            }

            //Mensagens de erro para os campos obrigatorios

            var mensagensErro = new List<string>();

            if (string.IsNullOrWhiteSpace(psicologo.Name))
                mensagensErro.Add("O campo 'Nome' é obrigatório.");
            if (string.IsNullOrWhiteSpace(psicologo.CPF))
                mensagensErro.Add("O campo 'CPF' é obrigatório.");
            if (string.IsNullOrWhiteSpace(psicologo.CRP))
                mensagensErro.Add("O campo 'CRP' é obrigatório.");

            if (mensagensErro.Any())
                throw new Exception(string.Join(", ", mensagensErro));

            await _psicologoRepository.AddPsicologoAsync(psicologo);
        }

        public async Task UpdatePsicologoAsync(Psicologo psicologo)
        {
            var psicologos = await _psicologoRepository.GetAllPsicologosAsync();
            //Pesquisa de psicologo pelo ID, CPF, Email ou Nome
            var existing = psicologos.FirstOrDefault(u =>
            u.Id == psicologo.Id ||
            u.CPF == psicologo.CPF ||
            u.Email.Equals(psicologo.Email, StringComparison.OrdinalIgnoreCase) ||
            (!string.IsNullOrWhiteSpace(psicologo.Name) && u.Name.Equals(psicologo.Name, StringComparison.OrdinalIgnoreCase)));

            if (existing == null)
            {
                throw new Exception("Pai não encontrado.");
            }
            await _psicologoRepository.UpdatePsicologoAsync(psicologo);
        }

        public async Task DesativarPsicologoAsync(int id)
        {
            var psicologos = await _psicologoRepository.GetAllPsicologosAsync();
            var psicologo = psicologos.FirstOrDefault(u => u.Id == id);

            if (psicologo == null)
            {
                throw new Exception("Psicologo não encontrado.");
            }
            if (!psicologo.Ativo)
            {
                throw new Exception("Psicologo já está desativado.");
            }

            await _psicologoRepository.DesativarPsicologoAsync(id);
        }
    }
}