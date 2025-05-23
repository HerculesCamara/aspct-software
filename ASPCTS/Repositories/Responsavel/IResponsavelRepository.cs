using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;

namespace ASPCTS.Repositories
{
    public interface IResponsavelRepository
    {
        Task<IEnumerable<Responsavel>> GetAllResponsaveisAsync();
        Task<Responsavel?> GetResponsavelByIdAsync(int id);
        Task<Responsavel?> GetResponsavelByEmailAsync(string email);
        Task<IEnumerable<Responsavel>> GetResponsaveisByCPFAsync(string cpf);
        Task AddResponsavelAsync(Responsavel responsavel);
        Task UpdateResponsavelAsync(Responsavel responsavel);
        Task DesativarResponsavelAsync(int id);
    }
}