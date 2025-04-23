using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCTS.DTOs.Psicologo
{
    public class PsicologoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Tipo { get; set; } = "Psicologo";
        public string CPF { get; set; } = string.Empty;
        public DateTimeOffset DataNascimento { get; set; }
        public string CRP { get; set; } = string.Empty;
    }
}