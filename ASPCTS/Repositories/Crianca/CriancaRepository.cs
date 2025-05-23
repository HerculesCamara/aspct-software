using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Context;
using ASPCTS.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPCTS.Repositories
{
    public class CriancaRepository : ICriancaRepository
    {
        private readonly ApplicationDbContext _context;

        public CriancaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Crianca>> GetAllCriancasAsync()
        {
            return await _context.Criancas
                .Include(c => c.Pai)
                .Include(c => c.Psicologo)
                .Include(c => c.Atividades)
                .ToListAsync();
        }

        public async Task<Crianca?> GetCriancaByIdAsync(int id)
        {
            return await _context.Criancas.FirstOrDefaultAsync(c => c.Id == id);
        }



        public async Task<IEnumerable<Crianca>> GetCriancaByNameAsync(string name)
        {
            return await _context.Criancas
                .Where(c => c.Nome.Contains(name))
                .Include(c => c.Pai)
                .Include(c => c.Psicologo)
                .Include(c => c.Atividades)
                .ToListAsync();
        }

        public async Task<Crianca?> GetCriancaByPaiIdAsync(int idPai)
        {
            return await _context.Criancas
                .Include(c => c.Pai)
                .Include(c => c.Psicologo)
                .Include(c => c.Atividades)
                .FirstOrDefaultAsync(c => c.PaiId == idPai);
        }

        public async Task AddCriancaAsync(Crianca crianca)
        {
            await _context.Criancas.AddAsync(crianca);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCriancaAsync(Crianca crianca)
        {
            _context.Criancas.Update(crianca);
            await _context.SaveChangesAsync();
        }

        public async Task DesativarCriancaAsync(int id)
        {
            var crianca = await GetCriancaByIdAsync(id);
            if (crianca != null)
            {
                crianca.Ativo = false; // Desativar a criança
                _context.Criancas.Update(crianca);
                // Desativar todas as atividades associadas
                var atividades = await _context.Atividades
                    .Where(a => a.CriancaId == crianca.Id)
                    .ToListAsync();
                foreach (var atividade in atividades)
                {
                    atividade.Ativo = false; // Desativar a atividade
                    _context.Atividades.Update(atividade);
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> UsuarioTemAcessoACriancaAsync(int criancaId, int userId, string userRole)
        {
            var crianca = await _context.Criancas.FindAsync(criancaId);
            if (crianca == null) return false;

            return userRole switch
            {
                "Responsavel" => crianca.PaiId == userId || crianca.MaeId == userId,
                "Psicologo" => crianca.PsicologoId == userId,
                _ => false
            };
        }

    }
}