namespace ListaDeTarefa.Domain.Events
{
    public record TarefaAtribuidaEvento(
        Guid TarefaId,
        Guid PessoaId,
        string Descricao,
        DateTimeOffset OcorridoEm) : IEventoDominio;
}
