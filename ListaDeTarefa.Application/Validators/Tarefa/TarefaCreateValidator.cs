using ListaDeTarefa.Application.DTOs.Tarefa;
using FluentValidation;

namespace ListaDeTarefa.Application.Validators.Tarefa
{
	public class TarefaCreateValidator : AbstractValidator<TarefaCreateDto>
	{
		public TarefaCreateValidator()
		{
			RuleFor(x => x.Descricao)
				.NotEmpty().WithMessage("A descrição é obrigatória.")
				.MaximumLength(200).WithMessage("A descrição deve ter no máximo 200 caracteres.");
		}
	}
}
