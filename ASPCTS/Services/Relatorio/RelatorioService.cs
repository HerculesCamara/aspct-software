using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;
using ASPCTS.Repositories;

namespace ASPCTS.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly IRelatorioRepository _repository;

        public RelatorioService(IRelatorioRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Relatorio>> GetAllRelatorioAsync()
        {
            return await _repository.GetAllRelatorioAsync();
        }

        public async Task<Relatorio?> GetRelatorioByIdAsync(int id)
        {
            return await _repository.GetRelatorioByIdAsync(id);
        }

        public async Task AddRelatorioAsync(Relatorio relatorio)
        {
            await _repository.AddRelatorioAsync(relatorio);
        }

        public async Task UpdateRelatorioAsync(Relatorio relatorio)
        {
            await _repository.UpdateRelatorioAsync(relatorio);
        }

        public async Task DesativarRelatorioAsync(int id)
        {
            await _repository.DesativarRelatorioAsync(id);
        }
    }
}