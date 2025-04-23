using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;
using ASPCTS.Repositories;

namespace ASPCTS.Services
{
    public class AtividadeService : IAtividadeService
    {
        private readonly IAtividadeRepository _atividadeRepository;
        private readonly ICriancaRepository _criancaRepository;

        public AtividadeService(IAtividadeRepository atividadeRepository, ICriancaRepository criancaRepository)
        {
            _atividadeRepository = atividadeRepository;
            _criancaRepository = criancaRepository;
        }

        public async Task<IEnumerable<Atividade>> GetAllAtividadesAsync()
        {
            return await _atividadeRepository.GetAllAtividadesAsync();
        }

        public async Task<Atividade?> GetAtividadeByIdAsync(int id)
        {
            var atividade = await _atividadeRepository.GetAtividadeByIdAsync(id);
            return atividade;
        }

        public async Task<IEnumerable<Atividade>> GetAtividadeByTituloAsync(string titulo)
        {
            return await _atividadeRepository.GetAtividadeByTituloAsync(titulo);
        }

        public async Task AddAtividadeAsync(Atividade atividade)
        {
            // Verificar se todos os campos obrigatórios estão preenchidos
            if (string.IsNullOrWhiteSpace(atividade.Titulo) ||
                string.IsNullOrWhiteSpace(atividade.Descricao) ||
                atividade.DataConclusao == default(DateTime) ||
                atividade.CriancaId == default(int))
            {
                throw new ArgumentException("Todos os campos obrigatórios devem ser preenchidos.");
            }

            // Verificar se a criança associada existe
            var crianca = await _criancaRepository.GetCriancaByIdAsync(atividade.CriancaId);
            if (crianca == null)
            {
                throw new ArgumentException("A criança associada à atividade não foi encontrada.");
            }

            try
            {
                await _atividadeRepository.AddAtividadeAsync(atividade);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao adicionar a atividade.", ex);
            }
        }


        public async Task UpdateAtividadeAsync(Atividade atividade)
        {
            //Permitir que o usuario altera somente o status de conclusão da atividade
            if (atividade.Concluida == default(bool))
            {
                throw new ArgumentException("O status de conclusão deve ser informado.");
            }
            await _atividadeRepository.UpdateAtividadeAsync(atividade);
        }

        public async Task DeleteAtividadeAsync(int id)
        {
            //Verificar se a atividade existe antes de tentar deletá-la
            var atividade = await _atividadeRepository.GetAtividadeByIdAsync(id);
            if (atividade == null)
            {
                throw new KeyNotFoundException($"Atividade com ID {id} não encontrada.");
            }
            //Permitir que o usuario deleta somente atividades que não foram concluídas 
            if (atividade.Concluida.GetValueOrDefault())
            {
                throw new InvalidOperationException("Atividades concluídas não podem ser deletadas.");
            }
            await _atividadeRepository.DeleteAtividadeAsync(id);
        }
    }
}