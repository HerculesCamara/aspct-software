using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
        public DbSet<Responsavel> Responsaveis { get; set; } = null!;
        public DbSet<Psicologo> Psicologos { get; set; } = null!;
        public DbSet<Relatorio> Relatorios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração da tabela de usuários
            modelBuilder.Entity<Usuario>()
                .ToTable("Usuarios")
                .HasDiscriminator<string>("Tipo")
                .HasValue<Responsavel>("Responsavel")
                .HasValue<Psicologo>("Psicologo");

            //Relacionamento: Crianca -> Pai (Usuario)
            modelBuilder.Entity<Crianca>()
                .HasOne(c => c.Pai)
                .WithMany()
                .HasForeignKey(c => c.PaiId)
                .OnDelete(DeleteBehavior.Restrict);
            //Relacionamento: Crianca -> Mae (Usuario)
            modelBuilder.Entity<Crianca>()
                .HasOne(c => c.Mae)
                .WithMany()
                .HasForeignKey(c => c.MaeId)
                .OnDelete(DeleteBehavior.Restrict);
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

            var converter = new ValueConverter<List<string>, string>(
    v => JsonSerializer.Serialize(v, default(JsonSerializerOptions)),
    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());

            var comparer = new ValueComparer<List<string>>(
                (c1, c2) => (c1 ?? new List<string>()).SequenceEqual(c2 ?? new List<string>()),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            modelBuilder.Entity<Relatorio>()
                .Property(r => r.MarcosAlcancados)
                .HasConversion(converter)
                .Metadata.SetValueComparer(comparer);
        }
    }
}