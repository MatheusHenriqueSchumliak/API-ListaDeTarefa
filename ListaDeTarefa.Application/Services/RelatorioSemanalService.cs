using ListaDeTarefa.Application.DTOs.RelatorioSemanal;
using ListaDeTarefa.Application.Factories;
using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Application.Interfaces.IService;
using ListaDeTarefa.Domain.Entities;

namespace ListaDeTarefa.Application.Services
{
    public class RelatorioSemanalService(
        ITarefaRepository tarefaRepository,
        IRelatorioSemanalRepository relatorioSemanalRepository,
        CalculadoraPeriodoSemana calculadoraPeriodoSemana) : IRelatorioSemanalService
    {
        #region Construtor
        private readonly ITarefaRepository _tarefaRepository = tarefaRepository;
        private readonly IRelatorioSemanalRepository _relatorioSemanalRepository = relatorioSemanalRepository;
        private readonly CalculadoraPeriodoSemana _calculadoraPeriodoSemana = calculadoraPeriodoSemana;
        #endregion Construtor

        public async Task<RelatorioSemanalDto?> BuscarUltimo()
        {
            var relatorio = await _relatorioSemanalRepository.BuscarUltimo();
            return relatorio is null ? null : RelatorioSemanalFactory.EntidadeParaDto(relatorio);
        }

        public async Task<RelatorioSemanalDto> GerarRelatorioSemanal(DateTimeOffset referencia)
        {
            var periodo = _calculadoraPeriodoSemana.CalcularSemanaAnterior(referencia);

            // Regra: Idempotência - não duplicar o relatório da mesma semana
            var existente = await _relatorioSemanalRepository.BuscarPorInicioSemana(periodo.Inicio);
            if (existente != null)
                return RelatorioSemanalFactory.EntidadeParaDto(existente);

            var resumo = await _tarefaRepository.ObterResumoPorPeriodo(periodo.Inicio, periodo.Fim);

            // Regra: Taxa de conclusão = concluídas na semana / (concluídas na semana + pendentes ao final)
            var baseDeCalculo = resumo.TotalConcluidas + resumo.TotalPendentesAoFinal;
            var taxaConclusao = baseDeCalculo == 0
                ? 0m
                : Math.Round((decimal)resumo.TotalConcluidas / baseDeCalculo * 100, 2);

            var relatorio = RelatorioSemanal.Criar(
                periodo.Inicio,
                periodo.Fim,
                resumo.TotalCriadas,
                resumo.TotalConcluidas,
                resumo.TotalPendentesAoFinal,
                taxaConclusao,
                resumo.TempoMedioConclusaoHoras);

            await _relatorioSemanalRepository.Adicionar(relatorio);
            return RelatorioSemanalFactory.EntidadeParaDto(relatorio);
        }
    }
}
