using System.Net;
using System.Net.Mail;
using ListaDeTarefa.Application.Interfaces;
using ListaDeTarefa.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ListaDeTarefa.Infrastructure.Notificacoes
{
    public class EmailNotificador(IOptions<EmailOptions> options, ILogger<EmailNotificador> logger) : INotificador
    {
        #region Construtor
        private readonly EmailOptions _options = options.Value;
        private readonly ILogger<EmailNotificador> _logger = logger;
        #endregion

        public async Task EnviarAsync(Pessoa pessoa, string mensagem, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_options.Host) || string.IsNullOrWhiteSpace(_options.Remetente))
                throw new InvalidOperationException("Configuração de e-mail incompleta (Host e Remetente são obrigatórios).");

            using var cliente = new SmtpClient(_options.Host, _options.Porta)
            {
                EnableSsl = _options.UsarSsl,
                Credentials = new NetworkCredential(_options.Usuario, _options.Senha)
            };

            using var email = new MailMessage(_options.Remetente, pessoa.Email, "Nova tarefa atribuída", mensagem);

            await cliente.SendMailAsync(email, cancellationToken);

            _logger.LogInformation("E-mail enviado para a pessoa {PessoaId}.", pessoa.Id);
        }
    }
}
