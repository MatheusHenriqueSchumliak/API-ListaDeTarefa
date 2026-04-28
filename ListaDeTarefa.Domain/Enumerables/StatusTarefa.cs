using System.Text.Json.Serialization;

namespace ListaDeTarefa.Domain.Enumerables
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum StatusTarefa
	{
		Pendente = 'P',
		Concluido = 'C'
	}
}
