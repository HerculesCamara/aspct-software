using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Context;
using ASPCTS.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPCTS.Repositories
{
    public class ResponsavelRepository : IResponsavelRepository
    {
        private readonly ApplicationDbContext _context;

        public ResponsavelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Responsavel>> GetAllResponsaveisAsync()
        {
            return await _context.Usuarios.
                OfType<Responsavel>().
                ToListAsync();
        }

        public async Task<Responsavel?> GetResponsavelByIdAsync(int id)
        {
            return await _context.Usuarios.
                OfType<Responsavel>().
                FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddResponsavelAsync(Responsavel responsavel)
        {
            await _context.Usuarios.AddAsync(responsavel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateResponsavelAsync(Responsavel responsavel)
        {
            _context.Usuarios.Update(responsavel);
            await _context.SaveChangesAsync();
        }

        public async Task DesativarResponsavelAsync(int id)
        {
            var responsavel = await GetResponsavelByIdAsync(id);
            if (responsavel != null)
            {
                responsavel.Ativo = false; // Desativa o Responsavel
                _context.Usuarios.Update(responsavel);
                await _context.SaveChangesAsync();
            }
        }
    }
}