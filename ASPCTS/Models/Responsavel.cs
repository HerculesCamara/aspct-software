using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCTS.Models
{
    public class Responsavel : Usuario
    {
        // Relacionamento com as crianças
        public ICollection<Crianca> Criancas { get; set; } = new List<Crianca>();


        public int? PsicologoId { get; set; }
        public Psicologo? Psicologo { get; set; }

        //Responsavel é pai ou mãe?
        public string Parentesco
        {
            get
            {
                return Sexo == Genero.Masculino ? "Pai" : "Mãe";
            }
        }
    }
}