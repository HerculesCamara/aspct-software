using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCTS.DTOs.Relatorio
{
    public class RelatorioCreateDTO
    {
        public int CriancaId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Resumo { get; set; } = string.Empty;
        public List<string> MarcosAlcancados { get; set; } = new();
        public string Observacoes { get; set; } = string.Empty;
        public string RecomendacoesCasa { get; set; } = string.Empty;
        public string RecomendacoesEscola { get; set; } = string.Empty;
    }
}