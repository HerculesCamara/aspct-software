using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;

namespace ASPCTS.Repositories
{
    public interface IPsicologoRepository
    {
        Task<IEnumerable<Psicologo>> GetAllPsicologosAsync();
        Task<Psicologo?> GetPsicologoByIdAsync(int id);
        Task<IEnumerable<Psicologo>> GetPsicologoByNameAsync(string name);
        Task<Psicologo?> GetPsicologoByCPFAsync(string cpf);
        Task<Psicologo?> GetPsicologoByCrpAsync(string crp);
        Task AddPsicologoAsync(Psicologo psicologo);
        Task UpdatePsicologoAsync(Psicologo psicologo);
        Task DesativarPsicologoAsync(int id);
    }
}