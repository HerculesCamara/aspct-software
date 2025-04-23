using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCTS.DTOs.Atividade
{
    public class AtividadeUpdateDTO
    {
        public bool Concluida { get; set; }
        public DateTime? DataConclusao { get; set; }
    }
}