using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCTS.DTOs.Responsavel
{
    public class ResponsavelUpdateDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? CPF { get; set; }
        public enum Genero
        {
            Masculino,
            Feminino
        }
        public DateTimeOffset? DataNascimento { get; set; }
    }
}