using ListaDeTarefa.Application.DTOs.Tarefa;
using FluentValidation;

namespace ListaDeTarefa.Application.Validators.Tarefa
{
	public class TarefaUpdateValidator : AbstractValidator<TarefaUpdateDto>
	{
		public TarefaUpdateValidator()
		{
			RuleFor(x => x.Descricao)
				.NotEmpty().WithMessage("A descrição é obrigatória.")
				.MaximumLength(200);
		}
	}
}
