using ListaDeTarefa.Infrastructure.Context;
using ListaDeTarefa.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using ListaDeTarefa.Domain.Entities;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Repository
{
    public class EstatisticaTarefaRepositoryTests
    {
        private EstatisticaTarefaRepository CriarRepository()
        {
            var options = new DbContextOptionsBuilder<ListaDeTarefaContexto>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new EstatisticaTarefaRepository(new ListaDeTarefaContexto(options));
        }

        private static EstatisticaTarefa CriarEstatistica(Guid tarefaId)
        {
            var criacao = DateTimeOffset.UtcNow.AddMinutes(-30);
            return EstatisticaTarefa.Criar(tarefaId, null, criacao, DateTimeOffset.UtcNow);
        }

        [Fact]
        public async Task BuscarPorTarefaId_DeveEncontrarEstatistica()
        {
            var repository = CriarRepository();
            var tarefaId = Guid.NewGuid();
            await repository.Adicionar(CriarEstatistica(tarefaId));
            await repository.Adicionar(CriarEstatistica(Guid.NewGuid()));

            var resultado = await repository.BuscarPorTarefaId(tarefaId);

            Assert.NotNull(resultado);
            Assert.Equal(tarefaId, resultado!.TarefaId);
            Assert.Equal(30, resultado.DuracaoEmMinutos, 0);
        }

        [Fact]
        public async Task BuscarPorTarefaId_DeveRetornarNulo_QuandoNaoExiste()
        {
            var repository = CriarRepository();

            var resultado = await repository.BuscarPorTarefaId(Guid.NewGuid());

            Assert.Null(resultado);
        }
    }
}
