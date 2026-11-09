using Cronos;
using ListaDeTarefa.Application.Interfaces;
using ListaDeTarefa.Infrastructure.Jobs.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ListaDeTarefa.Infrastructure.Jobs
{
    public class AgendadorJobsService(
        IServiceScopeFactory scopeFactory,
        IOptions<JobsOptions> options,
        TimeProvider timeProvider,
        ILogger<AgendadorJobsService> logger) : BackgroundService
    {
        #region Construtor
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private readonly JobsOptions _options = options.Value;
        private readonly TimeProvider _timeProvider = timeProvider;
        private readonly ILogger<AgendadorJobsService> _logger = logger;
        #endregion Construtor

        private static readonly TimeSpan _esperaMaxima = TimeSpan.FromMinutes(30);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var fuso = TimeZoneInfo.FindSystemTimeZoneById(_options.FusoHorario);
            var agenda = MontarAgenda();

            if (agenda.Count == 0)
            {
                _logger.LogWarning("Nenhum job agendado. Agendador encerrado.");
                return;
            }

            var proximas = agenda.ToDictionary(a => a.Key, a => CalcularProxima(a.Value, fuso));

            foreach (var (nome, proxima) in proximas)
                _logger.LogInformation("Job {Nome} agendado. Próxima execução: {Proxima}.", nome, proxima);

            while (!stoppingToken.IsCancellationRequested)
            {
                var proximaGeral = proximas.Values.Where(p => p.HasValue).Select(p => p!.Value).DefaultIfEmpty().Min();
                if (proximaGeral == default)
                    return;

                var espera = proximaGeral - _timeProvider.GetUtcNow();
                if (espera > TimeSpan.Zero)
                {
                    try
                    {
                        await Task.Delay(espera < _esperaMaxima ? espera : _esperaMaxima, _timeProvider, stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }

                var agora = _timeProvider.GetUtcNow();
                var vencidos = proximas.Where(p => p.Value.HasValue && p.Value.Value <= agora).Select(p => p.Key).ToList();

                foreach (var nome in vencidos)
                {
                    await ExecutarJobAsync(nome, stoppingToken);
                    proximas[nome] = CalcularProxima(agenda[nome], fuso);
                }
            }

            _logger.LogInformation("Agendador de jobs finalizado.");
        }

        private Dictionary<string, CronExpression> MontarAgenda()
        {
            var agenda = new Dictionary<string, CronExpression>();

            using var scope = _scopeFactory.CreateScope();
            var jobs = scope.ServiceProvider.GetServices<IJob>();

            foreach (var job in jobs)
            {
                if (!_options.Agendamentos.TryGetValue(job.Nome, out var agendamento) || !agendamento.Ativo)
                {
                    _logger.LogWarning("Job {Nome} sem agendamento ativo no appsettings. Não será executado.", job.Nome);
                    continue;
                }

                if (!CronExpression.TryParse(agendamento.Cron, out var cron))
                {
                    _logger.LogError("Cron inválido para o job {Nome}: '{Cron}'.", job.Nome, agendamento.Cron);
                    continue;
                }

                agenda[job.Nome] = cron;
            }

            return agenda;
        }

        private DateTimeOffset? CalcularProxima(CronExpression cron, TimeZoneInfo fuso)
        {
            return cron.GetNextOccurrence(_timeProvider.GetUtcNow(), fuso);
        }

        private async Task ExecutarJobAsync(string nome, CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var job = scope.ServiceProvider.GetServices<IJob>().FirstOrDefault(j => j.Nome == nome);

                if (job is null)
                {
                    _logger.LogError("Job {Nome} não encontrado na DI.", nome);
                    return;
                }

                await job.ExecutarAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao executar o job {Nome} no agendador.", nome);
            }
        }
    }
}
