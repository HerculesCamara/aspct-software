using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.DTOs.Atividade;
using ASPCTS.Models;

namespace ASPCTS.Services
{
    public interface IAtividadeService
    {
        Task<IEnumerable<Atividade>> GetAllAtividadesAsync();
        Task<Atividade?> GetAtividadeByIdAsync(int id);
        Task<IEnumerable<Atividade>> BuscarAtividadePorCriancaId(int criancaId);
        Task AddAtividadeAsync(Atividade Atividade);
        Task UpdateAtividadeAsync(Atividade Atividade);
        Task DesativarAtividadeAsync(int id);
    }
}