
using ListaDeTarefa.Application.Commom;
using ListaDeTarefa.Application.DependencyInjection;
using ListaDeTarefa.DependencyInjection;
using ListaDeTarefa.Infrastructure.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Text.Json.Serialization;

namespace ListaDeTarefa
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			#region OpenTelemetry

			builder.Services.AddOpenTelemetry().WithTracing(tracing =>
			{
				tracing.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("lista-de-tarefa-tracing"))
					   .SetSampler(new AlwaysOnSampler())
					   .AddSource(TelemetryHelper.ActivitySourceName)
					   .AddAspNetCoreInstrumentation(opt =>
					   {
						   opt.RecordException = true;
					   })
					   .AddHttpClientInstrumentation(opt =>
					   {
						   opt.RecordException = true;
					   })
					   .AddEntityFrameworkCoreInstrumentation()
					   ;

				if (builder.Environment.IsDevelopment())
				{
					tracing.AddConsoleExporter();
				}
			});
			//}).WithMetrics(metrics =>
			//{
			//	metrics.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("lista-de-tarefa-métrica"))
			//		   .AddMeter(Telemetry.MeterName)
			//		   .AddAspNetCoreInstrumentation()
			//		   .AddHttpClientInstrumentation()
			//		   .AddRuntimeInstrumentation()
			//		   .AddOtlpExporter(otl =>
			//		   {
			//			   otl.Endpoint = new Uri(builder.Configuration["OpenTelemetry:Endpoint"]!);
			//		   });

			//	if (builder.Environment.IsDevelopment())
			//	{
			//		metrics.AddConsoleExporter();
			//	}
			//});
			#endregion

			builder.Services.AddControllers();

			builder.Services.AddValidationServices();
			builder.Services.AddInfrastructureServices(builder.Configuration);

			builder.Services.AddProjectDependencies();

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
