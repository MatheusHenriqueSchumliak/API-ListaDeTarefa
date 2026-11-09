using ListaDeTarefa.Domain.Entities.Base;

namespace ListaDeTarefa.Domain.Entities
{
    public class RelatorioSemanal : EntityBase
    {
        public DateTimeOffset InicioSemana { get; private set; }
        public DateTimeOffset FimSemana { get; private set; }
        public int TotalCriadas { get; private set; }
        public int TotalConcluidas { get; private set; }
        public int TotalPendentes { get; private set; }
        public decimal TaxaConclusao { get; private set; }
        public decimal TempoMedioConclusaoHoras { get; private set; }
        public RelatorioSemanal() { }

        public static RelatorioSemanal Criar(DateTimeOffset inicioSemana, DateTimeOffset fimSemana, int totalCriadas, int totalConcluidas, int totalPendentes, decimal taxaConclusao, decimal tempoMedioConclusaoHoras)
        {
            if (fimSemana <= inicioSemana)
                throw new ArgumentException("O fim da semana deve ser maior que o início.", nameof(fimSemana));
            if (totalCriadas < 0)
                throw new ArgumentException("O total de criadas não pode ser negativo.", nameof(totalCriadas));
            if (totalConcluidas < 0)
                throw new ArgumentException("O total de concluídas não pode ser negativo.", nameof(totalConcluidas));
            if (totalPendentes < 0)
                throw new ArgumentException("O total de pendentes não pode ser negativo.", nameof(totalPendentes));
            if (taxaConclusao < 0 || taxaConclusao > 100)
                throw new ArgumentException("A taxa de conclusão deve estar entre 0 e 100.", nameof(taxaConclusao));
            if (tempoMedioConclusaoHoras < 0)
                throw new ArgumentException("O tempo médio de conclusão não pode ser negativo.", nameof(tempoMedioConclusaoHoras));

            return new RelatorioSemanal
            {
                InicioSemana = inicioSemana.ToUniversalTime(),
                FimSemana = fimSemana.ToUniversalTime(),
                TotalCriadas = totalCriadas,
                TotalConcluidas = totalConcluidas,
                TotalPendentes = totalPendentes,
                TaxaConclusao = taxaConclusao,
                TempoMedioConclusaoHoras = tempoMedioConclusaoHoras
            };
        }
    }
}
