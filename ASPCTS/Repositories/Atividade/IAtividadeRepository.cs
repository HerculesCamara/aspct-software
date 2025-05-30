using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;

namespace ASPCTS.Repositories
{
    public interface IAtividadeRepository
    {
        Task<IEnumerable<Atividade>> GetAllAtividadesAsync();
        Task<Atividade?> GetAtividadeByIdAsync(int id);
        Task<IEnumerable<Atividade>> BuscarAtividadePorCriancaId(int criancaId);
        Task<IEnumerable<Atividade>> GetAtividadeByTituloAsync(string name);
        Task AddAtividadeAsync(Atividade Atividade);
        Task UpdateAtividadeAsync(Atividade Atividade);
        Task DesativarAtividadeAsync(int id);
    }
}