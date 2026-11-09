using ListaDeTarefa.Domain.Entities;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Entities
{
    public class EstatisticaTarefaTests
    {
        [Fact]
        public void Criar_DeveCalcularDuracaoEmMinutos()
        {
            // Arrange
            var criacao = new DateTimeOffset(2025, 1, 1, 10, 0, 0, TimeSpan.Zero);
            var conclusao = criacao.AddMinutes(90);
            var tarefaId = Guid.NewGuid();
            var pessoaId = Guid.NewGuid();

            // Act
            var estatistica = EstatisticaTarefa.Criar(tarefaId, pessoaId, criacao, conclusao);

            // Assert
            Assert.Equal(tarefaId, estatistica.TarefaId);
            Assert.Equal(pessoaId, estatistica.PessoaId);
            Assert.Equal(conclusao, estatistica.DataConclusao);
            Assert.Equal(90, estatistica.DuracaoEmMinutos);
        }

        [Fact]
        public void Criar_DevePermitirPessoaNula()
        {
            var criacao = DateTimeOffset.UtcNow.AddMinutes(-5);

            var estatistica = EstatisticaTarefa.Criar(Guid.NewGuid(), null, criacao, DateTimeOffset.UtcNow);

            Assert.Null(estatistica.PessoaId);
        }

        [Fact]
        public void Criar_DeveGravarDataConclusaoEmUtc()
        {
            var criacao = new DateTimeOffset(2025, 1, 1, 7, 0, 0, TimeSpan.FromHours(-3));
            var conclusao = criacao.AddMinutes(10);

            var estatistica = EstatisticaTarefa.Criar(Guid.NewGuid(), null, criacao, conclusao);

            Assert.Equal(TimeSpan.Zero, estatistica.DataConclusao.Offset);
            Assert.Equal(10, estatistica.DuracaoEmMinutos);
        }

        [Fact]
        public void Criar_DeveLancarExcecao_QuandoTarefaIdVazio()
        {
            var agora = DateTimeOffset.UtcNow;

            Assert.Throws<ArgumentException>(() => EstatisticaTarefa.Criar(Guid.Empty, null, agora, agora));
        }

        [Fact]
        public void Criar_DeveLancarExcecao_QuandoConclusaoAnteriorACriacao()
        {
            var agora = DateTimeOffset.UtcNow;

            Assert.Throws<ArgumentException>(() => EstatisticaTarefa.Criar(Guid.NewGuid(), null, agora, agora.AddMinutes(-1)));
        }
    }
}
