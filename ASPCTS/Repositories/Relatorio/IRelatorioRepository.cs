using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;

namespace ASPCTS.Repositories
{
    public interface IRelatorioRepository
    {
        IQueryable<Relatorio> GetQueryableRelatorios();
        Task<IEnumerable<Relatorio>> GetAllRelatorioAsync();
        Task<Relatorio?> GetRelatorioByIdAsync(int id);
        Task<IEnumerable<Relatorio>> GetRelatorioByCriancaIdAsync(int criancaId);
        Task AddRelatorioAsync(Relatorio relatorio);
        Task UpdateRelatorioAsync(Relatorio relatorio);
        Task DesativarRelatorioAsync(int id);
    }
}