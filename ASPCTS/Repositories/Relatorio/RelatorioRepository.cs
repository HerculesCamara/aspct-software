using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ASPCTS.Context;
using ASPCTS.Models;

namespace ASPCTS.Repositories
{
    public class RelatorioRepository : IRelatorioRepository
    {
        private readonly ApplicationDbContext _context;

        public RelatorioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Relatorio> GetQueryableRelatorios()
        {
            // Retorna um IQueryable para permitir filtragem no service/controller
            return _context.Relatorios.AsNoTracking().Where(r => r.Ativo);
        }

        public async Task<IEnumerable<Relatorio>> GetAllRelatorioAsync()
        {
            return await _context.Relatorios.ToListAsync();
        }

        public async Task<Relatorio?> GetRelatorioByIdAsync(int id)
        {
            return await _context.Relatorios
                .AsNoTracking()
                .Include(r => r.Crianca)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Relatorio>> GetRelatorioByCriancaIdAsync(int criancaId)
        {
            return await _context.Relatorios
                .Where(r => r.CriancaId == criancaId && r.Ativo)
                .ToListAsync();
        }

        public async Task AddRelatorioAsync(Relatorio relatorio)
        {   
            relatorio.Data = DateTime.UtcNow; // Define a data atual como data do relatório
            relatorio.Ativo = true; // Define o relatório como ativo ao ser criado
            _context.Relatorios.Add(relatorio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRelatorioAsync(Relatorio relatorio)
        {
            _context.Relatorios.Update(relatorio);
            await _context.SaveChangesAsync();
        }

        public async Task DesativarRelatorioAsync(int id)
        {
            var relatorio = await _context.Relatorios.FindAsync(id);
            if (relatorio != null)
            {
                relatorio.Ativo = false; // Marca o relatório como inativo
                _context.Relatorios.Update(relatorio);
                await _context.SaveChangesAsync();
            }
        }
    }
}