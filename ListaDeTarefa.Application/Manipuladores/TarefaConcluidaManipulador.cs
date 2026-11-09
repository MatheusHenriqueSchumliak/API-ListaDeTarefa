using ListaDeTarefa.Application.Interfaces;
using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Domain.Entities;
using ListaDeTarefa.Domain.Events;

namespace ListaDeTarefa.Application.Manipuladores
{
    public class TarefaConcluidaManipulador(IEstatisticaTarefaRepository estatisticaRepository) : IManipuladorEvento<TarefaConcluidaEvento>
    {
        #region Construtor
        private readonly IEstatisticaTarefaRepository _estatisticaRepository = estatisticaRepository;
        #endregion

        public async Task ManipularAsync(TarefaConcluidaEvento evento, CancellationToken cancellationToken = default)
        {
            var existente = await _estatisticaRepository.BuscarPorTarefaId(evento.TarefaId);

            if (existente is not null)
                return;

            var estatistica = EstatisticaTarefa.Criar(evento.TarefaId, evento.PessoaId, evento.DataCriacao, evento.DataConclusao);

            await _estatisticaRepository.Adicionar(estatistica);
        }
    }
}
