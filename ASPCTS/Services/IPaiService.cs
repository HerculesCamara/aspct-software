using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;

namespace ASPCTS.Services
{
    public interface IPaiService
    {
        Task<IEnumerable<Pai>> GetAllPaisAsync();
        Task<Pai?> GetPaiByIdAsync(int id);
        Task<Pai?> GetPaiByCPFAsync(string cpf);
        Task AddPaiAsync(Pai pai);
        Task UpdatePaiAsync(Pai pai);
        Task DeletePaiAsync(int id);
    }
}