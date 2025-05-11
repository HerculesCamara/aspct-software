using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCTS.DTOs.Atividade
{
    public class AtividadeUpdateDTO
    {
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public bool? Concluida { get; set; }

    }
}