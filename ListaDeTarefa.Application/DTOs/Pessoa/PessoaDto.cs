namespace ListaDeTarefa.Application.DTOs.Pessoa
{
	public class PessoaDto
	{
		public Guid Id { get; set; }
		public string Nome { get; set; } = string.Empty;
		public string Sobrenome { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Telefone { get; set; } = string.Empty;
		public string WhatsApp { get; set; } = string.Empty;
		public DateTimeOffset DataNascimento { get; set; }
	}
}
