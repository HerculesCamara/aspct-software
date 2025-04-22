using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPCTS.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Models.Atividade> Atividades { get; set; } = null!;
        public DbSet<Models.Crianca> Criancas { get; set; } = null!;
        public DbSet<Models.Usuario> Usuarios { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            // Configuração da tabela de usuários
            modelBuilder.Entity<Usuario>()
                .ToTable("Usuarios")
                .HasDiscriminator<string>("Tipo")
                .HasValue<Pai>("Pai")
                .HasValue<Psicologo>("Psicologo");

            //Relacionamento: Crianca -> Pai (Usuario)
            modelBuilder.Entity<Crianca>()
                .HasOne(c => c.Pai)
                .WithMany(p => p.Criancas)
                .HasForeignKey(c => c.PaiId)
                .OnDelete(DeleteBehavior.Restrict); // Impede a exclusão em cascata do pai se houver filhos associados

            //Relacionamento: Crianca -> Psicologo (Usuario)
            modelBuilder.Entity<Crianca>()
                .HasOne(c => c.Psicologo)
                .WithMany(p => p.Criancas)
                .HasForeignKey(c => c.PsicologoId)
                .OnDelete(DeleteBehavior.Restrict); // Impede a exclusão em cascata do psicólogo se houver crianças associadas

            //Relacionamento: Atividade -> Crianca
            modelBuilder.Entity<Atividade>()
                .HasOne(a => a.Crianca)
                .WithMany(c => c.Atividades)
                .HasForeignKey(a => a.CriancaId)
                .OnDelete(DeleteBehavior.Restrict); // Impede a exclusão em cascata da criança se houver atividades associadas

            base.OnModelCreating(modelBuilder);
        }
    }
}