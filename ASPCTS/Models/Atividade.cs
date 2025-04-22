using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCTS.Models
{
    public class Atividade
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataConclusao { get; set; }
        public bool? Concluida { get; set; } = false;

        // Relacionamento com a criança
        public int CriancaId { get; set; }
        public Crianca? Crianca { get; set; }
    }
}