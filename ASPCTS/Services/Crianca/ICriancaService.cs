using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;

namespace ASPCTS.Services
{
    public interface ICriancaService
    {
        Task<IEnumerable<Crianca>> GetAllCriancasAsync();
        Task<Crianca?> GetCriancaByIdAsync(int id);
        Task AddCriancaAsync(Crianca crianca);
        Task UpdateCriancaAsync(Crianca crianca);
        Task DesativarCriancaAsync(int id);
    }
}