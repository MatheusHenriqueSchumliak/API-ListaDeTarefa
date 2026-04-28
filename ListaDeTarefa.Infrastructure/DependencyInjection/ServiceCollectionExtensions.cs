

using ListaDeTarefa.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ListaDeTarefa.Infrastructure.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddProjectDependencies(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<ListaDeTarefaContexto>(options =>
				options.UseSqlServer(connectionString));

			services.AddScoped<ITarefaRepository, TarefaRepository>();
			services.AddScoped<ITarefaService, TarefaService>();


			return services;
		}
	}
}