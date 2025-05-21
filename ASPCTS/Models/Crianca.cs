using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCTS.Models
{
    public class Crianca
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade => CalcularIdade();
        public DateTimeOffset DataNascimento { get; set; } = DateTimeOffset.UtcNow;
        public bool Ativo { get; set; } = true;

        // Relacionamento com Pai
        public int PaiId { get; set; }
        public virtual Responsavel? Pai { get; set; }
        // Relacionamento com Mãe
        public int MaeId { get; set; }
        public virtual Responsavel? Mae { get; set; }

        // Relacionamento com Psicólogo (opcional)
        public int PsicologoId { get; set; }
        public virtual Psicologo? Psicologo { get; set; }

        // Lista de atividades associadas
        public ICollection<Atividade> Atividades { get; set; } = new List<Atividade>();
        

        internal object Select(Func<object, object> value)
        {
            throw new NotImplementedException();
        }

        // Método para calcular a idade com base na data de nascimento
        private int CalcularIdade()
        {
            var hoje = DateTimeOffset.UtcNow;
            var idade = hoje.Year - DataNascimento.Year;
            if (DataNascimento > hoje.AddYears(-idade))
            {
                idade--;
            }

            return idade;
        }
    }
}