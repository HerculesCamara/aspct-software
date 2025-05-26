using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;

namespace ASPCTS.Repositories
{
    public interface IResponsavelRepository
    {
        Task<Responsavel?> GetByIdAsync(int id);
        Task UpdateAsync(Responsavel responsavel);
        Task<Responsavel?> GetResponsavelComCriancasAsync(int id);
        Task<IEnumerable<Crianca>> GetCriancasByResponsavelIdAsync(int responsavelId);
        Task<IEnumerable<Atividade>> GetAtividadesByResponsavelIdAsync(int responsavelId);
        Task<IEnumerable<Relatorio>> GetRelatoriosByResponsavelIdAsync(int responsavelId);
        Task<Psicologo?> GetPsicologoByResponsavelIdAsync(int responsavelId);
        Task<IEnumerable<Responsavel>> GetResponsaveisByPsicologoIdAsync(int psicologoId);
        Task<Responsavel?> GetResponsavelByIdAndPsicologoIdAsync(int responsavelId, int psicologoId);
        Task<Responsavel?> GetResponsavelByIdAsync(int id);
        Task<Responsavel?> GetResponsavelByEmailAsync(string email);
    }


}