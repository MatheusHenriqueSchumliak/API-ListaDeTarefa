using ListaDeTarefa.Domain.Entities.Base;
using ListaDeTarefa.Domain.Enumerables;

namespace ListaDeTarefa.Domain.Entities
{
	public class Tarefa : EntityBase
	{
		public string Descricao { get; private set; }
		public StatusTarefa Status { get; private set; }
		public DateTimeOffset? DataConclusao { get; private set; }

		public Tarefa() { }

		public Tarefa Criar(string descricao)
		{
			if (string.IsNullOrWhiteSpace(descricao))
				throw new ArgumentException("Descrição obrigatória.");

			return new Tarefa
			{
				Descricao = descricao,
				Status = StatusTarefa.Pendente,
				DataCriacao = DateTimeOffset.UtcNow
			};
		}

		public void AlterarDescricao(string novaDescricao)
		{
			if (Status == StatusTarefa.Concluido)
				throw new InvalidOperationException("Não é possível alterar uma tarefa concluída.");

			if (string.IsNullOrWhiteSpace(novaDescricao))
				throw new ArgumentException("Descrição obrigatória.");

			Descricao = novaDescricao;
			DataAtualizacao = DateTime.UtcNow;
		}

		public void Concluir()
		{
			if (Status == StatusTarefa.Concluido)
				throw new InvalidOperationException("Tarefa já está concluída.");

			Status = StatusTarefa.Concluido;
			DataConclusao = DateTimeOffset.UtcNow;
			DataAtualizacao = DateTimeOffset.UtcNow;
		}
	}
}
