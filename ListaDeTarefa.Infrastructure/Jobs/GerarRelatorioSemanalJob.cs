using ListaDeTarefa.Application.Interfaces.IService;
using Microsoft.Extensions.Logging;

namespace ListaDeTarefa.Infrastructure.Jobs
{
    public class GerarRelatorioSemanalJob(
        IRelatorioSemanalService relatorioSemanalService,
        TimeProvider timeProvider,
        ILogger<GerarRelatorioSemanalJob> logger) : BaseJob(logger)
    {
        #region Construtor
        private readonly IRelatorioSemanalService _relatorioSemanalService = relatorioSemanalService;
        private readonly TimeProvider _timeProvider = timeProvider;
        #endregion Construtor

        public override string Nome => "GerarRelatorioSemanalJob";

        protected override async Task ExecutarJobAsync(CancellationToken cancellationToken)
        {
            var relatorio = await _relatorioSemanalService.GerarRelatorioSemanal(_timeProvider.GetUtcNow());

            _logger.LogInformation(
                "Relatório da semana {InicioSemana:u} a {FimSemana:u}: {TotalCriadas} criadas, {TotalConcluidas} concluídas, {TotalPendentes} pendentes, taxa {TaxaConclusao}%.",
                relatorio.InicioSemana, relatorio.FimSemana, relatorio.TotalCriadas, relatorio.TotalConcluidas, relatorio.TotalPendentes, relatorio.TaxaConclusao);
        }
    }
}
