using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCTS.DTOs.Crianca
{
    public class CriancaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public int Idade { get; set; }
        public int PaiId { get; set; }
        public int MaeId { get; set; }
        public int? PsicologoId { get; set; }

    }
}