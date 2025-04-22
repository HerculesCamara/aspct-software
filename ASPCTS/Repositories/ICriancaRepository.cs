using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;

namespace ASPCTS.Repositories
{
    public interface ICriancaRepository
    {
        Task<IEnumerable<Crianca>> GetAllCriancasAsync();
        Task<Crianca?> GetCriancaByIdAsync(int id);
        Task<IEnumerable<Crianca>> GetCriancaByNameAsync(string name);
        Task<Crianca?> GetCriancaByPaiIdAsync(int idPai);
        Task AddCriancaAsync(Crianca crianca);
        Task UpdateCriancaAsync(Crianca crianca);
        Task DeleteCriancaAsync(int id);
    }
}