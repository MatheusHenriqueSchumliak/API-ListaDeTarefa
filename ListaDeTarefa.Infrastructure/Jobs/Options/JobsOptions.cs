namespace ListaDeTarefa.Infrastructure.Jobs.Options
{
    public class JobsOptions
    {
        public const string Secao = "Jobs";

        public string FusoHorario { get; set; } = "America/Sao_Paulo";
        public Dictionary<string, JobAgendamentoOptions> Agendamentos { get; set; } = [];
    }

    public class JobAgendamentoOptions
    {
        public string Cron { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }
}
