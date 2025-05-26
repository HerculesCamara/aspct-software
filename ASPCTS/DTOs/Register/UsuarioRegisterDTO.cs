using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;

namespace ASPCTS.DTOs.Register
{
    public class UsuarioRegisterDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // "Psicologo" ou "Responsavel"
        public Usuario.Genero Sexo { get; set; } // 0 = Masculino, 1 = Feminino
        public DateTimeOffset DataNascimento { get; set; }

        // Somente se for Psicólogo
        public string? CRP { get; set; }

        // Somente se for Responsável
        public int? PsicologoId { get; set; }
    }
}