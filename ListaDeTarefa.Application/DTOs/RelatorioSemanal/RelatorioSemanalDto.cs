namespace ListaDeTarefa.Application.DTOs.RelatorioSemanal
{
    public class RelatorioSemanalDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset InicioSemana { get; set; }
        public DateTimeOffset FimSemana { get; set; }
        public int TotalCriadas { get; set; }
        public int TotalConcluidas { get; set; }
        public int TotalPendentes { get; set; }
        public decimal TaxaConclusao { get; set; }
        public decimal TempoMedioConclusaoHoras { get; set; }
        public DateTimeOffset DataCriacao { get; set; }
    }
}
