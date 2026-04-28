using ListaDeTarefa.Application.Validators.Tarefa;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace ListaDeTarefa.Application.DependencyInjection
{
	public static class ValidationServiceCollectionExtensions
	{
		public static IServiceCollection AddValidationServices(this IServiceCollection services)
		{
			services.AddFluentValidationAutoValidation();
			services.AddValidatorsFromAssemblyContaining<TarefaCreateValidator>();
			services.AddValidatorsFromAssemblyContaining<TarefaUpdateValidator>();
			return services;
		}
	}
}
