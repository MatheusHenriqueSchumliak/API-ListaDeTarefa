using System.Text.Json.Serialization;

namespace ListaDeTarefa.Domain.Enumerables
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum TipoPessoa
	{
		Cliente = 'C',
		Profissional = 'P'
	}

}
