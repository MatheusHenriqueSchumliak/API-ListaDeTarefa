namespace ListaDeTarefa.Application.DTOs.EstatisticaTarefa
{
    public class EstatisticaTarefaDto
    {
        public Guid Id { get; set; }
        public Guid TarefaId { get; set; }
        public Guid? PessoaId { get; set; }
        public DateTimeOffset DataConclusao { get; set; }
        public double DuracaoEmMinutos { get; set; }
    }
}
