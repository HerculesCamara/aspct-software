using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;

namespace ASPCTS.DTOs.Responsavel
{
    public class ResponsavelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public Usuario.Genero Sexo { get; set; }
        public DateTimeOffset DataNascimento { get; set; }
        public int PsicologoId { get; set; }
        public bool Ativo { get; set; }
    }
}