using ListaDeTarefa.Application.DTOs.RelatorioSemanal;
using ListaDeTarefa.Domain.Entities;

namespace ListaDeTarefa.Application.Factories
{
    public static class RelatorioSemanalFactory
    {
        public static RelatorioSemanalDto EntidadeParaDto(RelatorioSemanal relatorio) => new()
        {
            Id = relatorio.Id,
            InicioSemana = relatorio.InicioSemana,
            FimSemana = relatorio.FimSemana,
            TotalCriadas = relatorio.TotalCriadas,
            TotalConcluidas = relatorio.TotalConcluidas,
            TotalPendentes = relatorio.TotalPendentes,
            TaxaConclusao = relatorio.TaxaConclusao,
            TempoMedioConclusaoHoras = relatorio.TempoMedioConclusaoHoras,
            DataCriacao = relatorio.DataCriacao
        };
    }
}
