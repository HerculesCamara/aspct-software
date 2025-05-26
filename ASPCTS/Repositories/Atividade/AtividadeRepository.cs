using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Context;
using ASPCTS.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPCTS.Repositories
{
    public class AtividadeRepository : IAtividadeRepository
    {
        private readonly ApplicationDbContext _context;

        public AtividadeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Atividade>> GetAllAtividadesAsync()
        {
            return await _context.Atividades
                .Include(a => a.Crianca)
                .ToListAsync();
        }

        public async Task<Atividade?> GetAtividadeByIdAsync(int id)
        {
            return await _context.Atividades
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Atividade>> BuscarAtividadePorCriancaId(int criancaId)
        {
            return await _context.Atividades
                .Where(a => a.CriancaId == criancaId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Atividade>> GetAtividadeByTituloAsync(string titulo)
        {
            return await _context.Atividades
                .Where(a => a.Titulo.Contains(titulo))
                .ToListAsync();
        }

        public async Task AddAtividadeAsync(Atividade atividade)
        {
            await _context.Atividades.AddAsync(atividade);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAtividadeAsync(Atividade atividade)
        {
            _context.Atividades.Update(atividade);
            await _context.SaveChangesAsync();
        }

        public async Task DesativarAtividadeAsync(int id)
        {
            var atividade = await GetAtividadeByIdAsync(id);
            if (atividade != null)
            {
                atividade.Ativo = false;
                _context.Atividades.Update(atividade);
                await _context.SaveChangesAsync();
            }
        }

    }
}