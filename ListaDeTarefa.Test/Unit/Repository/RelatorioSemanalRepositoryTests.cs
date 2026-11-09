using ListaDeTarefa.Infrastructure.Context;
using ListaDeTarefa.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using ListaDeTarefa.Domain.Entities;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Repository
{
    public class RelatorioSemanalRepositoryTests
    {
        private RelatorioSemanalRepository CriarRepository()
        {
            var options = new DbContextOptionsBuilder<ListaDeTarefaContexto>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new RelatorioSemanalRepository(new ListaDeTarefaContexto(options));
        }

        private static RelatorioSemanal CriarRelatorio(string inicio)
        {
            var inicioSemana = DateTimeOffset.Parse(inicio);
            return RelatorioSemanal.Criar(inicioSemana, inicioSemana.AddDays(7), 2, 1, 1, 50m, 10m);
        }

        [Fact]
        public async Task BuscarPorInicioSemana_DeveEncontrarRelatorio()
        {
            var repository = CriarRepository();
            await repository.Adicionar(CriarRelatorio("2025-06-02T03:00:00Z"));

            var resultado = await repository.BuscarPorInicioSemana(DateTimeOffset.Parse("2025-06-02T00:00:00-03:00"));

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado!.TotalCriadas);
        }

        [Fact]
        public async Task BuscarPorInicioSemana_DeveRetornarNulo_QuandoNaoExiste()
        {
            var repository = CriarRepository();

            var resultado = await repository.BuscarPorInicioSemana(DateTimeOffset.Parse("2025-06-02T03:00:00Z"));

            Assert.Null(resultado);
        }

        [Fact]
        public async Task BuscarUltimo_DeveRetornarORelatorioMaisRecente()
        {
            var repository = CriarRepository();
            await repository.Adicionar(CriarRelatorio("2025-06-02T03:00:00Z"));
            await repository.Adicionar(CriarRelatorio("2025-06-16T03:00:00Z"));
            await repository.Adicionar(CriarRelatorio("2025-06-09T03:00:00Z"));

            var resultado = await repository.BuscarUltimo();

            Assert.NotNull(resultado);
            Assert.Equal(DateTimeOffset.Parse("2025-06-16T03:00:00Z"), resultado!.InicioSemana);
        }

        [Fact]
        public async Task BuscarUltimo_DeveRetornarNulo_QuandoNaoHaRelatorios()
        {
            var repository = CriarRepository();

            var resultado = await repository.BuscarUltimo();

            Assert.Null(resultado);
        }
    }
}
