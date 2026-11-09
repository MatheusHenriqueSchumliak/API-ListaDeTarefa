namespace ListaDeTarefa.Infrastructure.Notificacoes
{
    public class EmailOptions
    {
        public const string Secao = "Email";

        public bool Ativo { get; set; } = false;
        public string Host { get; set; } = string.Empty;
        public int Porta { get; set; } = 587;
        public bool UsarSsl { get; set; } = true;
        public string Usuario { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Remetente { get; set; } = string.Empty;
    }
}
