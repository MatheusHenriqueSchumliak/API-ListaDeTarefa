using ListaDeTarefa.Application.DTOs.Tarefa;
using ListaDeTarefa.Application.Factories;
using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Application.Interfaces.IService;
using ListaDeTarefa.Application.Interfaces;
using ListaDeTarefa.Domain.Enumerables;

namespace ListaDeTarefa.Application.Services
{
	public class TarefaService(ITarefaRepository tarefaRepository, IPessoaRepository pessoaRepository, IDespachanteEventos despachanteEventos) : ITarefaService
	{
		#region Construtor
		private readonly ITarefaRepository _tarefaRepository = tarefaRepository;
		private readonly IPessoaRepository _pessoaRepository = pessoaRepository;
		private readonly IDespachanteEventos _despachanteEventos = despachanteEventos;
		#endregion Construtor

		public async Task<TarefaDto?> BuscarPorDescricao(string descricao)
		{
			var tarefa = await _tarefaRepository.BuscarPorDescricao(descricao);
			return tarefa is null ? null : TarefaFactory.EntidadeParaDto(tarefa);
		}

		public async Task<IEnumerable<TarefaDto>> BuscarTodos()
		{
			var tarefas = await _tarefaRepository.BuscarTodos();
			return tarefas.Select(TarefaFactory.EntidadeParaDto);
		}

		public async Task<TarefaDto?> BuscarPorId(Guid id)
		{
			var tarefa = await _tarefaRepository.BuscarPorId(id);
			return tarefa is null ? null : TarefaFactory.EntidadeParaDto(tarefa);
		}

		public async Task<TarefaDto> Adicionar(TarefaCreateDto createDto)
		{
			// Regra: Não permitir cadastro com descrição duplicada
			var existente = await _tarefaRepository.BuscarPorDescricao(createDto.Descricao);
			if (existente != null)
				throw new InvalidOperationException("Já existe uma tarefa com essa descrição.");

			var tarefa = TarefaFactory.createDtoParaEntidade(createDto);
			await _tarefaRepository.Adicionar(tarefa);
			return TarefaFactory.EntidadeParaDto(tarefa);
		}

		public async Task Atualizar(Guid id, TarefaUpdateDto updateDto)
		{
			var tarefa = await _tarefaRepository.BuscarPorId(id);
			if (tarefa is null)
				throw new KeyNotFoundException("Tarefa não encontrada.");

			// Regra: Não permitir descrição duplicada (exceto para a própria tarefa)
			var existente = await _tarefaRepository.BuscarPorDescricao(updateDto.Descricao);
			if (existente != null && existente.Id != id)
				throw new InvalidOperationException("Já existe uma tarefa com essa descrição.");

			// Validação: Só permite alterar para 'C'
			if (updateDto.Status != (char)StatusTarefa.Concluido)
				throw new InvalidOperationException("O status só pode ser alterado para 'C' (Concluído).");

			TarefaFactory.UpdateDtoParaEntidade(tarefa, updateDto);
			await _tarefaRepository.Atualizar(tarefa);

			var eventos = tarefa.Eventos.ToList();
			tarefa.LimparEventos();
			await _despachanteEventos.DespacharAsync(eventos);
		}

		public async Task AtribuirPessoa(Guid tarefaId, Guid pessoaId)
		{
			var tarefa = await _tarefaRepository.BuscarPorId(tarefaId);
			if (tarefa is null)
				throw new KeyNotFoundException("Tarefa não encontrada.");

			var pessoa = await _pessoaRepository.BuscarPorId(pessoaId);
			if (pessoa is null)
				throw new KeyNotFoundException("Pessoa não encontrada.");

			tarefa.AtribuirPessoa(pessoa);
			await _tarefaRepository.Atualizar(tarefa);

			var eventos = tarefa.Eventos.ToList();
			tarefa.LimparEventos();
			await _despachanteEventos.DespacharAsync(eventos);
		}

		public async Task Remover(Guid id)
		{
			var tarefa = await _tarefaRepository.BuscarPorId(id);
			if (tarefa is null)
				throw new KeyNotFoundException("Tarefa não encontrada.");

			await _tarefaRepository.Remover(tarefa);
		}
	}
}