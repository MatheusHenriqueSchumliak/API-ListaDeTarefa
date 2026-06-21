
using ListaDeTarefa.Infrastructure.DependencyInjection;
using ListaDeTarefa.Application.DependencyInjection;
using ListaDeTarefa.DependencyInjection;
using System.Text.Json.Serialization;

namespace ListaDeTarefa
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();

			builder.Services.AddValidationServices();
			builder.Services.AddInfrastructureServices(builder.Configuration);

			var connectionString = builder.Configuration.GetConnectionString("ListaDeTarefaConnection");

			builder.Services.AddProjectDependencies(connectionString!);

			builder.Services.AddControllers()
					.AddJsonOptions(options =>
					{
						options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
					});

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddSwaggerDocumentation();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerDocumentation();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
