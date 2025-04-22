using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Context;
using ASPCTS.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPCTS.Repositories
{
    public class PaiRepository : IPaiRepository
    {
         private readonly ApplicationDbContext _context;

        public PaiRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pai>> GetAllPaisAsync()
        {
            return await _context.Usuarios.
                OfType<Pai>().
                ToListAsync();
        }

        public async Task<Pai?> GetPaiByIdAsync(int id)
        {
            return await _context.Usuarios.
                OfType<Pai>().
                FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pai>> GetPaiByNameAsync(string name)
        {
            return await _context.Usuarios
                .Where(u => u.Name.Contains(name))
                .OfType<Pai>()
                .ToListAsync();
        }

        public async Task<Pai?> GetPaiByCPFAsync(string cpf)
        {
            return await _context.Usuarios
                .OfType<Pai>()
                .FirstOrDefaultAsync(p => p.CPF == cpf);
        }

        public async Task AddPaiAsync(Pai pai)
        {
            await _context.Usuarios.AddAsync(pai);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePaiAsync(Pai pai)
        {
            _context.Usuarios.Update(pai);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePaiAsync(int id)
        {
            var pai = await GetPaiByIdAsync(id);
            if (pai != null)
            {
                _context.Usuarios.Remove(pai);
                await _context.SaveChangesAsync();
            }
        }
    }
}