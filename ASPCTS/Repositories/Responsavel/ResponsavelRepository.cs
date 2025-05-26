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

        public async Task<Responsavel?> GetByIdAsync(int id) =>
            await _context.Responsaveis.FindAsync(id);

        public async Task UpdateAsync(Responsavel responsavel)
        {
            _context.Responsaveis.Update(responsavel);
            await _context.SaveChangesAsync();
        }

        public async Task<Responsavel?> GetResponsavelComCriancasAsync(int id) =>
            await _context.Responsaveis
                .Include(r => r.Criancas)
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<IEnumerable<Crianca>> GetCriancasByResponsavelIdAsync(int responsavelId) =>
            await _context.Criancas
                .Where(c => c.PaiId == responsavelId || c.MaeId == responsavelId)
                .ToListAsync();

        public async Task<IEnumerable<Atividade>> GetAtividadesByResponsavelIdAsync(int responsavelId) =>
            await _context.Atividades
                .Where(a => a.Crianca != null && (a.Crianca.PaiId == responsavelId || a.Crianca.MaeId == responsavelId))
                .Include(a => a.Crianca)
                .ToListAsync();

        public async Task<IEnumerable<Relatorio>> GetRelatoriosByResponsavelIdAsync(int responsavelId) =>
            await _context.Relatorios
                .Where(r => r.Crianca != null && (r.Crianca.PaiId == responsavelId || r.Crianca.MaeId == responsavelId))
                .Include(r => r.Crianca)
                .ToListAsync();

        public async Task<Psicologo?> GetPsicologoByResponsavelIdAsync(int responsavelId) =>
            await _context.Criancas
                .Where(c => c.PaiId == responsavelId || c.MaeId == responsavelId)
                .Select(c => c.Psicologo)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<Responsavel>> GetResponsaveisByPsicologoIdAsync(int psicologoId) =>
            await _context.Criancas
                .Where(c => c.PsicologoId == psicologoId)
                .SelectMany(c => new[] { c.Pai, c.Mae })
                .OfType<Responsavel>()
                .Distinct()
                .ToListAsync();

        public async Task<Responsavel?> GetResponsavelByIdAndPsicologoIdAsync(int responsavelId, int psicologoId) =>
            await _context.Criancas
                .Where(c => c.PsicologoId == psicologoId &&
                           (c.PaiId == responsavelId || c.MaeId == responsavelId))
                .SelectMany(c => new[] { c.Pai, c.Mae })
                .FirstOrDefaultAsync(r => r != null && r.Id == responsavelId);

        public async Task<Responsavel?> GetResponsavelByIdAsync(int id) =>
            await _context.Responsaveis
                .Include(r => r.Criancas)
                .FirstOrDefaultAsync(r => r.Id == id);
        public async Task<Responsavel?> GetResponsavelByEmailAsync(string email) =>
            await _context.Responsaveis
                .FirstOrDefaultAsync(r => r.Email == email);
    }

}