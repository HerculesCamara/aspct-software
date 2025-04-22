using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;

namespace ASPCTS.Services
{
    public interface IAtividadeService
    {
        Task<IEnumerable<Atividade>> GetAllAtividadesAsync();
        Task<Atividade?> GetAtividadeByIdAsync(int id);
        Task AddAtividadeAsync(Atividade Atividade);
        Task UpdateAtividadeAsync(Atividade Atividade);
        Task DeleteAtividadeAsync(int id);
    }
}