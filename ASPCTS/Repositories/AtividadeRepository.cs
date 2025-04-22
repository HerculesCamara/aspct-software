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
                .Include(a => a.Crianca)
                .FirstOrDefaultAsync(a => a.Id == id);
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

        public async Task DeleteAtividadeAsync(int id)
        {
            var atividade = await GetAtividadeByIdAsync(id);
            if (atividade != null)
            {
                _context.Atividades.Remove(atividade);
                await _context.SaveChangesAsync();
            }
        }
    }
}