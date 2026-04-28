namespace ListaDeTarefa.Application.DTOs.Tarefa
{
	public class TarefaUpdateDto
	{
		public string Descricao { get; set; } = string.Empty;
		public char Status { get; set; }
	}
}
