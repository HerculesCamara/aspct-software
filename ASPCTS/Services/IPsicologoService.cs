using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;

namespace ASPCTS.Services
{
    public interface IPsicologoService
    {
        Task<IEnumerable<Psicologo>> GetAllPsicologosAsync();
        Task<Psicologo?> GetPsicologoByIdAsync(int id);
        Task<Psicologo?> GetPsicologoByCPFAsync(string cpf);
        Task AddPsicologoAsync(Psicologo psicologo);
        Task UpdatePsicologoAsync(Psicologo psicologo);
        Task DeletePsicologoAsync(int id);
    }
}