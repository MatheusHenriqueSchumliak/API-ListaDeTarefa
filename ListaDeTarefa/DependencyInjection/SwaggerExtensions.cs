using Microsoft.OpenApi.Models;

namespace ListaDeTarefa.DependencyInjection
{
	public static class SwaggerExtensions
	{
		public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lista de Tarefa API", Version = "v1" });

			});

			return services;
		}

		public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
		{
			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.DefaultModelsExpandDepth(-1); //Oculta os models da pagina do swagger.
			});

			return app;
		}
	}
}
