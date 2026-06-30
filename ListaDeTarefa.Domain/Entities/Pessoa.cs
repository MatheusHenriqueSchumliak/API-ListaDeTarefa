using ListaDeTarefa.Domain.Entities.Base;

namespace ListaDeTarefa.Domain.Entities
{
	public class Pessoa : EntityBase
	{
		public string Nome { get; private set; }
		public string Sobrenome { get; private set; }
		public string Email { get; private set; }
		public string Telefone { get; private set; }
		public string WhatsApp { get; private set; }
		public DateTimeOffset DataNascimento { get; private set; }

		public Pessoa() { }

		public static Pessoa Criar(string nome, string sobrenome, string email, string telefone, string whatsapp, DateTimeOffset dataNascimento)
		{
			if (string.IsNullOrWhiteSpace(nome))
				throw new ArgumentException("O nome não pode ser vazio.", nameof(nome));
			if (string.IsNullOrWhiteSpace(sobrenome))
				throw new ArgumentException("O sobrenome não pode ser vazio.", nameof(sobrenome));
			if (string.IsNullOrWhiteSpace(email))
				throw new ArgumentException("O email não pode ser vazio.", nameof(email));
			if (string.IsNullOrWhiteSpace(telefone))
				throw new ArgumentException("O telefone não pode ser vazio.", nameof(telefone));
			if (string.IsNullOrWhiteSpace(whatsapp))
				throw new ArgumentException("O WhatsApp não pode ser vazio.", nameof(whatsapp));

			return new Pessoa { Nome = nome, Sobrenome = sobrenome, Email = email, Telefone = telefone, WhatsApp = whatsapp, DataNascimento = dataNascimento };
		}



	}
}
