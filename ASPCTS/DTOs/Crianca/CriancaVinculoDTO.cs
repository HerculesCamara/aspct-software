using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCTS.DTOs.Crianca
{
    public class CriancaVinculoDTO
    {
        public int? PaiId { get; set; }
        public int? MaeId { get; set; }
        public int? PsicologoId { get; set; }
    }
}