using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCTS.Models
{
    public class Psicologo : Usuario
    {
        public string CRP { get; set; } = string.Empty; // Registro do CRP (Conselho Regional de Psicologia)

        // Relacionamento com crianças
        public ICollection<Crianca> Criancas { get; set; } = new List<Crianca>();
    
    }
}