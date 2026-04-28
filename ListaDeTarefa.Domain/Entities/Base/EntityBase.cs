namespace ListaDeTarefa.Domain.Entities.Base
{
	public class EntityBase
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public DateTimeOffset DataCriacao { get; set; } = DateTime.UtcNow;
		public DateTimeOffset? DataAtualizacao { get; set; } = null;
		public EntityBase()
		{
			Id = Guid.NewGuid();
			DataCriacao = DateTime.UtcNow;
			DataAtualizacao = null;
		}
	}
}
