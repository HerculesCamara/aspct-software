using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace ASPCTS.Models
{
    public abstract class Usuario
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;// Tipo de usuario (Psicologo, Pai, etc.)   
        public string CPF { get; set; } = string.Empty;
        public enum Genero
        {
            Masculino,
            Feminino
        }
        public Genero Sexo { get; set; }
        public DateTimeOffset DataNascimento { get; set; } = DateTimeOffset.UtcNow;
        public bool Ativo { get; set; } = true;

        public void SetPassword(string plainPassword)
        {
            Password = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        public bool VerifyPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, Password);
        }
    }
}