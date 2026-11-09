using ListaDeTarefa.Application.DTOs.EstatisticaTarefa;
using ListaDeTarefa.Domain.Entities;

namespace ListaDeTarefa.Application.Factories
{
    public static class EstatisticaTarefaFactory
    {
        public static EstatisticaTarefaDto EntidadeParaDto(EstatisticaTarefa estatistica) => new()
        {
            Id = estatistica.Id,
            TarefaId = estatistica.TarefaId,
            PessoaId = estatistica.PessoaId,
            DataConclusao = estatistica.DataConclusao,
            DuracaoEmMinutos = estatistica.DuracaoEmMinutos
        };
    }
}
