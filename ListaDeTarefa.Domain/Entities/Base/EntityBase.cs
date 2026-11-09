using ListaDeTarefa.Domain.Events;

namespace ListaDeTarefa.Domain.Entities.Base
{
	public class EntityBase
	{
		private readonly List<IEventoDominio> _eventos = [];

		public Guid Id { get; set; } = Guid.NewGuid();
		public DateTimeOffset DataCriacao { get; set; } = DateTime.UtcNow;
		public DateTimeOffset? DataAtualizacao { get; set; } = null;
		public IReadOnlyCollection<IEventoDominio> Eventos => _eventos.AsReadOnly();

		public EntityBase()
		{
			Id = Guid.NewGuid();
			DataCriacao = DateTime.UtcNow;
			DataAtualizacao = null;
		}

		protected void AdicionarEvento(IEventoDominio evento) => _eventos.Add(evento);

		public void LimparEventos() => _eventos.Clear();
	}
}
