using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Context;
using ASPCTS.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPCTS.Repositories
{
    public class PsicologoRepository : IPsicologoRepository
    {
        private readonly ApplicationDbContext _context;

        public PsicologoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Psicologo>> GetAllPsicologosAsync()
        {
            return await _context.Usuarios.
                OfType<Psicologo>().
                ToListAsync();
        }

        public async Task<Psicologo?> GetPsicologoByIdAsync(int id)
        {
            return await _context.Usuarios.
                OfType<Psicologo>().
                FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Psicologo>> GetPsicologoByNameAsync(string name)
        {
            return await _context.Usuarios
                .Where(u => u.Name.Contains(name))
                .OfType<Psicologo>()
                .ToListAsync();
        }

        public async  Task<Psicologo?> GetPsicologoByCPFAsync(string cpf)
        {
            return await _context.Usuarios.
                OfType<Psicologo>()
                .FirstOrDefaultAsync(p => p.CPF == cpf);
        }

        public async Task<Psicologo?> GetPsicologoByCrpAsync(string crp)
        {
            return await _context.Usuarios.
                OfType<Psicologo>()
                .FirstOrDefaultAsync(p => p.CRP == crp);
        }
        public async Task AddPsicologoAsync(Psicologo psicologo)
        {
            await _context.Usuarios.AddAsync(psicologo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePsicologoAsync(Psicologo psicologo)
        {
            _context.Usuarios.Update(psicologo);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePsicologoAsync(int id)
        {
            var psicologo = await GetPsicologoByIdAsync(id);
            if (psicologo != null)
            {
                _context.Usuarios.Remove(psicologo);
                await _context.SaveChangesAsync();
            }
        }
    }
}