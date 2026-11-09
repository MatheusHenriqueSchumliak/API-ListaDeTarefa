using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ListaDeTarefa.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ListaDeTarefa.Infrastructure.Mapping
{
    public class RelatorioSemanalMapping : IEntityTypeConfiguration<RelatorioSemanal>
    {
        public void Configure(EntityTypeBuilder<RelatorioSemanal> builder)
        {
            builder.ToTable("RelatoriosSemanais");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.InicioSemana).IsRequired();

            builder.Property(r => r.FimSemana).IsRequired();

            builder.Property(r => r.TotalCriadas).IsRequired();

            builder.Property(r => r.TotalConcluidas).IsRequired();

            builder.Property(r => r.TotalPendentes).IsRequired();

            builder.Property(r => r.TaxaConclusao)
                .IsRequired()
                .HasPrecision(5, 2);

            builder.Property(r => r.TempoMedioConclusaoHoras)
                .IsRequired()
                .HasPrecision(12, 2);

            builder.HasIndex(r => r.InicioSemana)
                .IsUnique();
        }
    }
}
