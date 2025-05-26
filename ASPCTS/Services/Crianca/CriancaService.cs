using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ASPCTS.Models;
using ASPCTS.Repositories;

namespace ASPCTS.Services
{
    public class CriancaService : ICriancaService
    {
        private readonly ICriancaRepository _criancaRepository;
        private readonly IResponsavelRepository _responsavelRepository;
        public CriancaService(ICriancaRepository criancaRepository, IResponsavelRepository responsavelRepository)
        {
            _criancaRepository = criancaRepository;
            _responsavelRepository = responsavelRepository;
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

        public async Task<IEnumerable<Crianca>> GetCriancasPermitidasParaUsuarioAsync(int usuarioId)
        {
 
            return await _criancaRepository.GetCriancasPermitidasParaUsuarioAsync(usuarioId);
        }

        public async Task AddCriancaAsync(Crianca crianca)
        {
            var mensagensErro = new List<string>();

            if (string.IsNullOrWhiteSpace(crianca.Nome))
                mensagensErro.Add("O campo 'Nome' é obrigatório.");

            bool temPaiValido = false;
            bool temMaeValida = false;

            // Verifica Pai (se informado)
            if (crianca.PaiId != 0)
            {
                var pai = await _responsavelRepository.GetResponsavelByIdAsync(crianca.PaiId);
                if (pai == null)
                    mensagensErro.Add("Pai não encontrado.");
                else if (pai.Sexo != Usuario.Genero.Masculino)
                    mensagensErro.Add("O usuário selecionado como pai não é do sexo masculino.");
                else
                    temPaiValido = true;
            }

            // Verifica Mãe (se informado)
            if (crianca.MaeId != 0)
            {
                var mae = await _responsavelRepository.GetResponsavelByIdAsync(crianca.MaeId);
                if (mae == null)
                    mensagensErro.Add("Mãe não encontrada.");
                else if (mae.Sexo != Usuario.Genero.Feminino)
                    mensagensErro.Add("O usuário selecionado como mãe não é do sexo feminino.");
                else
                    temMaeValida = true;
            }

            if (!temPaiValido && !temMaeValida)
                mensagensErro.Add("A criança deve possuir ao menos um responsável válido (pai ou mãe).");

            if (mensagensErro.Any())
                throw new Exception(string.Join(" ", mensagensErro));

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

        public async Task<bool> UsuarioTemAcessoACriancaAsync(int criancaId, ClaimsPrincipal user)
        {
            if (user.Identity is not { IsAuthenticated: true }) return false;

            var userId = int.Parse(user.FindFirst("id")?.Value ?? "0");
            var userRole = user.FindFirst(ClaimTypes.Role)?.Value ?? "";

            return await _criancaRepository.UsuarioTemAcessoACriancaAsync(criancaId, userId, userRole);
        }

    }
}