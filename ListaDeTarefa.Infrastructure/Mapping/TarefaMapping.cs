using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ListaDeTarefa.Domain.Enumerables;
using ListaDeTarefa.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ListaDeTarefa.Infrastructure.Mapping
{
	public class TarefaMapping : IEntityTypeConfiguration<Tarefa>
	{
		public void Configure(EntityTypeBuilder<Tarefa> builder)
		{
			builder.ToTable("Tarefas");

			builder.HasKey(t => t.Id);

			builder.Property(t => t.Descricao)
				.IsRequired()
				.HasMaxLength(255);

			builder.Property(t => t.Status)
				.IsRequired()
				.HasConversion(
					v => (char)v, // Enum para char
					v => (StatusTarefa)v // char para Enum
				)
				.HasColumnType("char(1)");

			builder.Property(t => t.DataCriacao)
				.IsRequired();

			builder.Property(t => t.DataAtualizacao);

			builder.Property(t => t.DataConclusao);
		}
	}
}