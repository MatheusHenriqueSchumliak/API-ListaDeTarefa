using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using ListaDeTarefa.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

namespace ListaDeTarefa.Test.Fixtures
{
	public class CustomWebApplicationFactory : WebApplicationFactory<Program>
	{
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(services =>
			{
				// Remove o DbContext existente
				services.RemoveAll(typeof(DbContextOptions<ListaDeTarefaContexto>));

				// Adiciona um banco de dados InMemory para testes
				services.AddDbContext<ListaDeTarefaContexto>(options =>
				{
					options.UseInMemoryDatabase("TestDatabase");
				});

				// Garante que o banco seja criado
				var serviceProvider = services.BuildServiceProvider();
				using var scope = serviceProvider.CreateScope();
				var db = scope.ServiceProvider.GetRequiredService<ListaDeTarefaContexto>();
				db.Database.EnsureCreated();
			});
		}
	}
}