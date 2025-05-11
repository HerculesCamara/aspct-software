using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;

namespace ASPCTS.Repositories
{
    public interface IPaiRepository
    {
        Task<IEnumerable<Pai>> GetAllPaisAsync();
        Task<Pai?> GetPaiByIdAsync(int id);
        Task<IEnumerable<Pai>> GetPaiByNameAsync(string name);
        Task<Pai?> GetPaiByCPFAsync(string cpf);
        Task AddPaiAsync(Pai pai);
        Task UpdatePaiAsync(Pai pai);
        Task DesativarPaiAsync(int id);
    }
}