using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Application.Interfaces.IService;
using ListaDeTarefa.Application.Interfaces;
using ListaDeTarefa.Infrastructure.Jobs;
using ListaDeTarefa.Infrastructure.Events;
using ListaDeTarefa.Infrastructure.Notificacoes;
using ListaDeTarefa.Application.Manipuladores;
using ListaDeTarefa.Domain.Events;
using ListaDeTarefa.Infrastructure.Jobs.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using ListaDeTarefa.Infrastructure.Repository;
using ListaDeTarefa.Infrastructure.Context;
using ListaDeTarefa.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ListaDeTarefa.Infrastructure.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("ListaDeTarefaConnection");
            // 🔍 DIAGNÓSTICO - Adicione estas linhas
            //Console.WriteLine("=================================");
            //Console.WriteLine($"Connection String: {connectionString}");
            //Console.WriteLine("=================================");

            services.AddDbContext<ListaDeTarefaContexto>(options =>
				options.UseNpgsql(connectionString)
					   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
			);

			services.AddMemoryCache();

			services.Configure<JobsOptions>(configuration.GetSection(JobsOptions.Secao));
			services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.Secao));
			services.AddSingleton<TimeProvider>(sp =>
				new TimeProviderComFuso(TimeZoneInfo.FindSystemTimeZoneById(sp.GetRequiredService<IOptions<JobsOptions>>().Value.FusoHorario)));
			services.AddHostedService<AgendadorJobsService>();
			services.AddHealthChecks().AddDbContextCheck<ListaDeTarefaContexto>("ListaDeTarefa DB");

			return services;
		}

		public static IServiceCollection AddProjectDependencies(this IServiceCollection services)
		{
			#region REPOSITORIES
			services.AddScoped<ITarefaRepository, TarefaRepository>();
			services.AddScoped<IPessoaRepository, PessoaRepository>();
			services.AddScoped<IRelatorioSemanalRepository, RelatorioSemanalRepository>();
			services.AddScoped<IEstatisticaTarefaRepository, EstatisticaTarefaRepository>();
			#endregion

			#region SERVICES
			services.AddScoped<ITarefaService, TarefaService>();
			services.AddScoped<IPessoaService, PessoaService>();
			services.AddScoped<IRelatorioSemanalService, RelatorioSemanalService>();
			services.AddScoped<IEstatisticaTarefaService, EstatisticaTarefaService>();
			services.AddScoped<CalculadoraPeriodoSemana>();
			#endregion

			#region EVENTOS
			services.AddScoped<DespachanteEventos>();
			services.AddSingleton<FilaEventos>();
			services.AddScoped<IDespachanteEventos, DespachanteEventosEmFila>();
			services.AddHostedService<ProcessadorEventosService>();
			services.AddScoped<LogNotificador>();
			services.AddScoped<EmailNotificador>();
			services.AddScoped<INotificador>(sp => sp.GetRequiredService<IOptions<EmailOptions>>().Value.Ativo
				? sp.GetRequiredService<EmailNotificador>()
				: sp.GetRequiredService<LogNotificador>());
			services.AddScoped<IManipuladorEvento<TarefaAtribuidaEvento>, TarefaAtribuidaManipulador>();
			services.AddScoped<IManipuladorEvento<TarefaConcluidaEvento>, TarefaConcluidaManipulador>();
			#endregion

			#region JOBS
			services.AddScoped<IJob, ExemploLogJob>();
			services.AddScoped<IJob, GerarRelatorioSemanalJob>();
			#endregion


			return services;
		}
	}
}