namespace ListaDeTarefa.Application.DTOs.Tarefa
{
	public class TarefaDto
	{
		public Guid Id { get; set; }
		public string Descricao { get; set; } = string.Empty;
		public char Status { get; set; }
		public DateTimeOffset DataCriacao { get; set; }
		public DateTimeOffset? DataAtualizacao { get; set; }
		public DateTimeOffset? DataConclusao { get; set; }
	}
}
