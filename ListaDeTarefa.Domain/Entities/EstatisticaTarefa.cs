using ListaDeTarefa.Domain.Entities.Base;

namespace ListaDeTarefa.Domain.Entities
{
    public class EstatisticaTarefa : EntityBase
    {
        public Guid TarefaId { get; private set; }
        public Guid? PessoaId { get; private set; }
        public DateTimeOffset DataConclusao { get; private set; }
        public double DuracaoEmMinutos { get; private set; }
        public EstatisticaTarefa() { }

        public static EstatisticaTarefa Criar(Guid tarefaId, Guid? pessoaId, DateTimeOffset dataCriacaoTarefa, DateTimeOffset dataConclusao)
        {
            if (tarefaId == Guid.Empty)
                throw new ArgumentException("O identificador da tarefa é obrigatório.", nameof(tarefaId));
            if (dataConclusao < dataCriacaoTarefa)
                throw new ArgumentException("A data de conclusão não pode ser anterior à data de criação.", nameof(dataConclusao));

            return new EstatisticaTarefa
            {
                TarefaId = tarefaId,
                PessoaId = pessoaId,
                DataConclusao = dataConclusao.ToUniversalTime(),
                DuracaoEmMinutos = (dataConclusao - dataCriacaoTarefa).TotalMinutes
            };
        }
    }
}
