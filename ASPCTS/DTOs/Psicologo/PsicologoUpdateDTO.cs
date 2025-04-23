using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCTS.DTOs.Psicologo
{
    public class PsicologoUpdateDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? CPF { get; set; }
        public DateTimeOffset? DataNascimento { get; set; }
        public string? CRP { get; set; }
    }
}