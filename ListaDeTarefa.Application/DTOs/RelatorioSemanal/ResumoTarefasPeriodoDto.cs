namespace ListaDeTarefa.Application.DTOs.RelatorioSemanal
{
    public record ResumoTarefasPeriodoDto(int TotalCriadas, int TotalConcluidas, int TotalPendentesAoFinal, decimal TempoMedioConclusaoHoras);
}
