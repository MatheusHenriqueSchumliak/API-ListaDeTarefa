using ListaDeTarefa.Application.Interfaces;
using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Domain.Events;

namespace ListaDeTarefa.Application.Manipuladores
{
    public class TarefaAtribuidaManipulador(IPessoaRepository pessoaRepository, INotificador notificador) : IManipuladorEvento<TarefaAtribuidaEvento>
    {
        #region Construtor
        private readonly IPessoaRepository _pessoaRepository = pessoaRepository;
        private readonly INotificador _notificador = notificador;
        #endregion

        public async Task ManipularAsync(TarefaAtribuidaEvento evento, CancellationToken cancellationToken = default)
        {
            var pessoa = await _pessoaRepository.BuscarPorId(evento.PessoaId);

            if (pessoa is null)
                return;

            var mensagem = $"Olá {pessoa.Nome}, a tarefa '{evento.Descricao}' foi atribuída a você";

            await _notificador.EnviarAsync(pessoa, mensagem, cancellationToken);
        }
    }
}
