using ListaDeTarefa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ListaDeTarefa.Infrastructure.Mapping
{
    public class EstatisticaTarefaMapping : IEntityTypeConfiguration<EstatisticaTarefa>
    {
        public void Configure(EntityTypeBuilder<EstatisticaTarefa> builder)
        {
            builder.ToTable("EstatisticasTarefa");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.TarefaId).IsRequired();

            builder.Property(e => e.PessoaId);

            builder.Property(e => e.DataConclusao).IsRequired();

            builder.Property(e => e.DuracaoEmMinutos).IsRequired();

            builder.Ignore(e => e.Eventos);

            builder.HasIndex(e => e.TarefaId)
                .IsUnique();

            builder.HasOne<Tarefa>()
                .WithMany()
                .HasForeignKey(e => e.TarefaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
