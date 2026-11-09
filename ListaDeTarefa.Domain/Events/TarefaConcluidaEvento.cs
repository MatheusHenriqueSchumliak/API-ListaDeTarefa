namespace ListaDeTarefa.Domain.Events
{
    public record TarefaConcluidaEvento(
        Guid TarefaId,
        Guid? PessoaId,
        DateTimeOffset DataCriacao,
        DateTimeOffset DataConclusao) : IEventoDominio;
}
