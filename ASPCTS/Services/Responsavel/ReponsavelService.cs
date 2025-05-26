using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.DTOs.Atividade;
using ASPCTS.DTOs.Crianca;
using ASPCTS.DTOs.Psicologo;
using ASPCTS.DTOs.Relatorio;
using ASPCTS.DTOs.Responsavel;
using ASPCTS.Models;
using ASPCTS.Repositories;
using AutoMapper;

namespace ASPCTS.Services
{
    public class ResponsavelService : IResponsavelService
    {
        private readonly IResponsavelRepository _repository;
        private readonly IMapper _mapper;

        public ResponsavelService(IResponsavelRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Responsavel?> GetResponsavelComCriancasAsync(int id) =>
            await _repository.GetResponsavelComCriancasAsync(id);

        public async Task<bool> AtualizarResponsavelAsync(int id, ResponsavelUpdateDTO dto)
        {
            var responsavel = await _repository.GetByIdAsync(id);
            if (responsavel == null) return false;

            _mapper.Map(dto, responsavel);
            await _repository.UpdateAsync(responsavel);
            return true;
        }

        public async Task<bool> DesativarResponsavelAsync(int id)
        {
            var responsavel = await _repository.GetByIdAsync(id);
            if (responsavel == null) return false;

            responsavel.Ativo = false;
            await _repository.UpdateAsync(responsavel);
            return true;
        }

        public async Task<IEnumerable<CriancaDTO>> GetCriancasDoResponsavelAsync(int responsavelId)
        {
            var criancas = await _repository.GetCriancasByResponsavelIdAsync(responsavelId);
            return _mapper.Map<IEnumerable<CriancaDTO>>(criancas);
        }

        public async Task<IEnumerable<AtividadeDTO>> GetAtividadesDasCriancasAsync(int responsavelId)
        {
            var atividades = await _repository.GetAtividadesByResponsavelIdAsync(responsavelId);
            return _mapper.Map<IEnumerable<AtividadeDTO>>(atividades);
        }

        public async Task<IEnumerable<RelatorioDTO>> GetRelatoriosDasCriancasAsync(int responsavelId)
        {
            var relatorios = await _repository.GetRelatoriosByResponsavelIdAsync(responsavelId);
            return _mapper.Map<IEnumerable<RelatorioDTO>>(relatorios);
        }

        public async Task<PsicologoDTO?> GetPsicologoDasCriancasAsync(int responsavelId)
        {
            var psicologo = await _repository.GetPsicologoByResponsavelIdAsync(responsavelId);
            return psicologo == null ? null : _mapper.Map<PsicologoDTO>(psicologo);
        }

        public async Task<IEnumerable<ResponsavelDTO>> GetResponsaveisPorPsicologoAsync(int psicologoId)
        {
            var responsaveis = await _repository.GetResponsaveisByPsicologoIdAsync(psicologoId);
            return _mapper.Map<IEnumerable<ResponsavelDTO>>(responsaveis);
        }

        public async Task<bool> AtualizarResponsavelPorPsicologoAsync(int psicologoId, int responsavelId, ResponsavelUpdateDTO dto)
        {
            var responsavel = await _repository.GetResponsavelByIdAndPsicologoIdAsync(responsavelId, psicologoId);
            if (responsavel == null) return false;

            _mapper.Map(dto, responsavel);
            await _repository.UpdateAsync(responsavel);
            return true;
        }
        public async Task<Responsavel?> GetResponsavelByEmailAsync(string email)
        {
            return await _repository.GetResponsavelByEmailAsync(email);
        }
    }

}