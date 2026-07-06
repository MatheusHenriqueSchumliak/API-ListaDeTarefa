using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Application.Interfaces.IService;
using Microsoft.Extensions.DependencyInjection;
using ListaDeTarefa.Infrastructure.Repository;
using ListaDeTarefa.Infrastructure.Context;
using ListaDeTarefa.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ListaDeTarefa.Infrastructure.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("ListaDeTarefaConnection");

			// 🔍 DIAGNÓSTICO - Adicione estas linhas
			Console.WriteLine("=================================");
			Console.WriteLine($"Connection String: {connectionString}");
			Console.WriteLine("=================================");

			services.AddDbContext<ListaDeTarefaContexto>(options =>
				options.UseSqlServer(connectionString)
					   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
			);

			services.AddMemoryCache();
			services.AddHealthChecks().AddDbContextCheck<ListaDeTarefaContexto>("ListaDeTarefa DB");

			return services;
		}

		public static IServiceCollection AddProjectDependencies(this IServiceCollection services)
		{
			#region REPOSITORIES
			services.AddScoped<ITarefaRepository, TarefaRepository>();
			services.AddScoped<IPessoaRepository, PessoaRepository>();
			#endregion

			#region SERVICES
			services.AddScoped<ITarefaService, TarefaService>();
			services.AddScoped<IPessoaService, PessoaService>();
			#endregion


			return services;
		}
	}
}