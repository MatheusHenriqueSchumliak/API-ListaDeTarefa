using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ListaDeTarefa.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ListaDeTarefa.Infrastructure.Mapping
{
	public class PessoaMapping : IEntityTypeConfiguration<Pessoa>
	{
		public void Configure(EntityTypeBuilder<Pessoa> builder)
		{
			builder.ToTable("Pessoas");

			builder.HasKey(p => p.Id);

			builder.Property(p => p.Nome)
				.IsRequired()
				.HasMaxLength(100);

			builder.Property(p => p.Sobrenome)
				.IsRequired()
				.HasMaxLength(100);

			builder.Property(p => p.Email)
				.IsRequired()
				.HasMaxLength(200);

			builder.Property(p => p.Telefone)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(p => p.WhatsApp)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(p => p.DataNascimento)
				.IsRequired();

			builder.Property(p => p.TipoPessoa)
				.IsRequired();

			builder.HasMany(p => p.Tarefas)
				.WithOne(t => t.Pessoa)
				.HasForeignKey(t => t.PessoaId)
				.OnDelete(DeleteBehavior.SetNull);
		}
	}
}
