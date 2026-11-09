using ListaDeTarefa.Infrastructure.Context;
using ListaDeTarefa.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using ListaDeTarefa.Domain.Entities;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Repository
{
    public class TarefaRepositoryResumoPorPeriodoTests
    {
        private static readonly DateTimeOffset _inicio = DateTimeOffset.Parse("2025-06-02T03:00:00Z");
        private static readonly DateTimeOffset _fim = DateTimeOffset.Parse("2025-06-09T03:00:00Z");

        private TarefaRepository CriarRepository()
        {
            var options = new DbContextOptionsBuilder<ListaDeTarefaContexto>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new TarefaRepository(new ListaDeTarefaContexto(options));
        }

        private static Tarefa CriarTarefa(string descricao, string criacao, string? conclusao = null)
        {
            var tarefa = Tarefa.Criar(descricao);
            tarefa.DataCriacao = DateTimeOffset.Parse(criacao);

            if (conclusao is not null)
            {
                tarefa.Concluir();
                typeof(Tarefa).GetProperty(nameof(Tarefa.DataConclusao))!.SetValue(tarefa, DateTimeOffset.Parse(conclusao));
            }

            return tarefa;
        }

        [Fact]
        public async Task ObterResumoPorPeriodo_DeveRetornarZeros_QuandoNaoHaTarefas()
        {
            var repository = CriarRepository();

            var resumo = await repository.ObterResumoPorPeriodo(_inicio, _fim);

            Assert.Equal(0, resumo.TotalCriadas);
            Assert.Equal(0, resumo.TotalConcluidas);
            Assert.Equal(0, resumo.TotalPendentesAoFinal);
            Assert.Equal(0m, resumo.TempoMedioConclusaoHoras);
        }

        [Fact]
        public async Task ObterResumoPorPeriodo_DeveContarApenasTarefasDoPeriodo()
        {
            var repository = CriarRepository();
            // Criada e concluída dentro da semana (24h)
            await repository.Adicionar(CriarTarefa("A", "2025-06-03T10:00:00Z", "2025-06-04T10:00:00Z"));
            // Criada na semana e ainda pendente
            await repository.Adicionar(CriarTarefa("B", "2025-06-05T10:00:00Z"));
            // Criada antes e concluída depois do fim: estava pendente ao final da semana
            await repository.Adicionar(CriarTarefa("C", "2025-05-20T10:00:00Z", "2025-06-10T10:00:00Z"));
            // Criada antes e concluída dentro da semana (336h)
            await repository.Adicionar(CriarTarefa("D", "2025-05-20T10:00:00Z", "2025-06-03T10:00:00Z"));
            // Criada depois do fim: fora do período
            await repository.Adicionar(CriarTarefa("E", "2025-06-12T10:00:00Z"));

            var resumo = await repository.ObterResumoPorPeriodo(_inicio, _fim);

            Assert.Equal(2, resumo.TotalCriadas);
            Assert.Equal(2, resumo.TotalConcluidas);
            Assert.Equal(2, resumo.TotalPendentesAoFinal);
            Assert.Equal(180m, resumo.TempoMedioConclusaoHoras);
        }

        [Fact]
        public async Task ObterResumoPorPeriodo_FimDeveSerExclusivo()
        {
            var repository = CriarRepository();
            await repository.Adicionar(CriarTarefa("No limite", "2025-06-09T03:00:00Z"));

            var resumo = await repository.ObterResumoPorPeriodo(_inicio, _fim);

            Assert.Equal(0, resumo.TotalCriadas);
        }
    }
}
