using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCTS.DTOs.Atividade
{
    public class AtividadeDTO
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Concluida { get; set; } 
        public DateTime DataConclusao { get; set; }
        public int CriancaId { get; set; }
        public string CriancaNome { get; set; } = string.Empty;
    }
}