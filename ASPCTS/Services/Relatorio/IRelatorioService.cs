using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;

namespace ASPCTS.Services
{
    public interface IRelatorioService
    {
        Task<IEnumerable<Relatorio>> GetAllRelatorioAsync();
        Task<Relatorio?> GetRelatorioByIdAsync(int id);
        Task AddRelatorioAsync(Relatorio relatorio);
        Task UpdateRelatorioAsync(Relatorio relatorio);
        Task DesativarRelatorioAsync(int id);
    }
}