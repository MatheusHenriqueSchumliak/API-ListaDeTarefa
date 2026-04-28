using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ListaDeTarefa.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ListaDeTarefa.Infrastructure.Context
{
	public class ListaDeTarefaContexto(DbContextOptions<ListaDeTarefaContexto> options) : DbContext(options)
	{
		public DbSet<Tarefa> Tarefas { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//Trata datas como UTC (a partir do EF Core 6 +)
			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				foreach (var property in entityType.GetProperties().Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
				{
					property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
						v => v, // Salva como está
						v => DateTime.SpecifyKind(v, DateTimeKind.Utc))); // Lê como UTC
				}
			}

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ListaDeTarefaContexto).Assembly);

			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new TarefaMapping());

		}
	}
}
