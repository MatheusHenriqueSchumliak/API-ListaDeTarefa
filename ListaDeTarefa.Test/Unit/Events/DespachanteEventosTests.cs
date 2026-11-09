using ListaDeTarefa.Application.Interfaces;
using ListaDeTarefa.Domain.Events;
using ListaDeTarefa.Infrastructure.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Events
{
    public class DespachanteEventosTests
    {
        private static readonly TarefaAtribuidaEvento _evento = new(Guid.NewGuid(), Guid.NewGuid(), "Tarefa", DateTimeOffset.UtcNow);

        private static DespachanteEventos CriarDespachante(params IManipuladorEvento<TarefaAtribuidaEvento>[] manipuladores)
        {
            var services = new ServiceCollection();
            foreach (var manipulador in manipuladores)
                services.AddSingleton(manipulador);

            return new DespachanteEventos(services.BuildServiceProvider(), NullLogger<DespachanteEventos>.Instance);
        }

        [Fact]
        public async Task DespacharAsync_DeveExecutarTodosOsManipuladores()
        {
            // Arrange
            var primeiro = new Mock<IManipuladorEvento<TarefaAtribuidaEvento>>();
            var segundo = new Mock<IManipuladorEvento<TarefaAtribuidaEvento>>();
            var despachante = CriarDespachante(primeiro.Object, segundo.Object);

            // Act
            await despachante.DespacharAsync([_evento]);

            // Assert
            primeiro.Verify(m => m.ManipularAsync(_evento, It.IsAny<CancellationToken>()), Times.Once);
            segundo.Verify(m => m.ManipularAsync(_evento, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DespacharAsync_ManipuladorQueFalha_NaoDerrubaOsOutros()
        {
            // Arrange
            var falho = new Mock<IManipuladorEvento<TarefaAtribuidaEvento>>();
            falho.Setup(m => m.ManipularAsync(It.IsAny<TarefaAtribuidaEvento>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("falha"));
            var saudavel = new Mock<IManipuladorEvento<TarefaAtribuidaEvento>>();
            var despachante = CriarDespachante(falho.Object, saudavel.Object);

            // Act
            var excecao = await Record.ExceptionAsync(() => despachante.DespacharAsync([_evento]));

            // Assert
            Assert.Null(excecao);
            saudavel.Verify(m => m.ManipularAsync(_evento, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DespacharAsync_DeveRegistrarErroEmLog_QuandoManipuladorFalha()
        {
            // Arrange
            var falho = new Mock<IManipuladorEvento<TarefaAtribuidaEvento>>();
            falho.Setup(m => m.ManipularAsync(It.IsAny<TarefaAtribuidaEvento>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("falha"));
            var logger = new Mock<ILogger<DespachanteEventos>>();
            var services = new ServiceCollection();
            services.AddSingleton(falho.Object);
            var despachante = new DespachanteEventos(services.BuildServiceProvider(), logger.Object);

            // Act
            await despachante.DespacharAsync([_evento]);

            // Assert
            logger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public async Task DespacharAsync_NaoDeveFazerNada_QuandoNaoHaManipuladores()
        {
            var despachante = CriarDespachante();

            var excecao = await Record.ExceptionAsync(() => despachante.DespacharAsync([_evento]));

            Assert.Null(excecao);
        }
    }
}
