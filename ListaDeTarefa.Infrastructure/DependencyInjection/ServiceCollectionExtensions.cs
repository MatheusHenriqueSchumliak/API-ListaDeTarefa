using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Application.Interfaces.IService;
using Microsoft.Extensions.DependencyInjection;
using ListaDeTarefa.Infrastructure.Repository;
using ListaDeTarefa.Infrastructure.Context;
using ListaDeTarefa.Application.Services;
using Microsoft.EntityFrameworkCore;

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