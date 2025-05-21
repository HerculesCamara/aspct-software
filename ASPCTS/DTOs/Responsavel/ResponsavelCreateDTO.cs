using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCTS.DTOs.Responsavel
{
    public class ResponsavelCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public enum Genero
        {
            Masculino,
            Feminino
        }
        public DateTimeOffset DataNascimento { get; set; }
        public int PsicologoId { get; set; }
    }
}