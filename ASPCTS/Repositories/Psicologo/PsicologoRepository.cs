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

        public async Task<Psicologo?> GetPsicologoByEmailAsync(string email)
        {
            return await _context.Usuarios.
                OfType<Psicologo>()
                .FirstOrDefaultAsync(p => p.Email == email);
        }
        public async Task<IEnumerable<Psicologo>> GetPsicologosByCPFAsync(string cpf)
        {
            return await _context.Usuarios.
                OfType<Psicologo>()
                .Where(p => p.CPF == cpf)
                .ToListAsync();
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

        public async Task DesativarPsicologoAsync(int id)
        {
            var psicologo = await GetPsicologoByIdAsync(id);
            if (psicologo != null)
            {
                psicologo.Ativo = false; // Desativa o psicólogo
                _context.Usuarios.Update(psicologo);
                await _context.SaveChangesAsync();
            }
        }
    }
}