using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.DTOs.Atividade;
using ASPCTS.DTOs.Crianca;
using ASPCTS.DTOs.Psicologo;
using ASPCTS.DTOs.Relatorio;
using ASPCTS.DTOs.Responsavel;
using ASPCTS.Models;

namespace ASPCTS.Services
{
    public interface IResponsavelService
    {
        Task<Responsavel?> GetResponsavelComCriancasAsync(int id);
        Task<bool> AtualizarResponsavelAsync(int id, ResponsavelUpdateDTO dto);
        Task<bool> DesativarResponsavelAsync(int id);
        Task<IEnumerable<CriancaDTO>> GetCriancasDoResponsavelAsync(int responsavelId);
        Task<IEnumerable<AtividadeDTO>> GetAtividadesDasCriancasAsync(int responsavelId);
        Task<IEnumerable<RelatorioDTO>> GetRelatoriosDasCriancasAsync(int responsavelId);
        Task<PsicologoDTO?> GetPsicologoDasCriancasAsync(int responsavelId);
        Task<IEnumerable<ResponsavelDTO>> GetResponsaveisPorPsicologoAsync(int psicologoId);
        Task<bool> AtualizarResponsavelPorPsicologoAsync(int psicologoId, int responsavelId, ResponsavelUpdateDTO dto);
        Task<Responsavel?> GetResponsavelByEmailAsync(string email);
    }

}