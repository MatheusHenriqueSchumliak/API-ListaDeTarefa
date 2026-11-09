using ListaDeTarefa.Application.Interfaces;
using ListaDeTarefa.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ListaDeTarefa.Infrastructure.Notificacoes
{
    public class LogNotificador(ILogger<LogNotificador> logger) : INotificador
    {
        #region Construtor
        private readonly ILogger<LogNotificador> _logger = logger;
        #endregion

        public Task EnviarAsync(Pessoa pessoa, string mensagem, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Notificação para {PessoaId}: {Mensagem}", pessoa.Id, mensagem);
            return Task.CompletedTask;
        }
    }
}
