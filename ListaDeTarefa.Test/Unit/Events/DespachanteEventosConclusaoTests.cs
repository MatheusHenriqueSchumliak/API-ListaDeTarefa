using ListaDeTarefa.Application.Interfaces;
using ListaDeTarefa.Application.Interfaces.IRepository;
using ListaDeTarefa.Application.Manipuladores;
using ListaDeTarefa.Domain.Entities;
using ListaDeTarefa.Domain.Events;
using ListaDeTarefa.Infrastructure.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Events
{
    public class DespachanteEventosConclusaoTests
    {
        private static readonly TarefaConcluidaEvento _evento =
            new(Guid.NewGuid(), null, DateTimeOffset.UtcNow.AddMinutes(-10), DateTimeOffset.UtcNow);

        [Fact]
        public async Task DespacharAsync_NaoDeveLancarExcecao_QuandoGravarEstatisticaFalha()
        {
            // Arrange
            var repositorio = new Mock<IEstatisticaTarefaRepository>();
            repositorio.Setup(r => r.BuscarPorTarefaId(It.IsAny<Guid>())).ReturnsAsync((EstatisticaTarefa?)null);
            repositorio.Setup(r => r.Adicionar(It.IsAny<EstatisticaTarefa>())).ThrowsAsync(new Exception("banco fora do ar"));
            var logger = new Mock<ILogger<DespachanteEventos>>();
            var services = new ServiceCollection();
            services.AddSingleton<IManipuladorEvento<TarefaConcluidaEvento>>(new TarefaConcluidaManipulador(repositorio.Object));
            var despachante = new DespachanteEventos(services.BuildServiceProvider(), logger.Object);

            // Act
            var excecao = await Record.ExceptionAsync(() => despachante.DespacharAsync([_evento]));

            // Assert
            Assert.Null(excecao);
            logger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public async Task DespacharAsync_OutroManipuladorDoMesmoEvento_ExecutaSemAlterarOManipuladorDeEstatistica()
        {
            // Arrange
            var repositorio = new Mock<IEstatisticaTarefaRepository>();
            repositorio.Setup(r => r.BuscarPorTarefaId(It.IsAny<Guid>())).ReturnsAsync((EstatisticaTarefa?)null);
            var outro = new Mock<IManipuladorEvento<TarefaConcluidaEvento>>();
            var services = new ServiceCollection();
            services.AddSingleton<IManipuladorEvento<TarefaConcluidaEvento>>(new TarefaConcluidaManipulador(repositorio.Object));
            services.AddSingleton(outro.Object);
            var despachante = new DespachanteEventos(services.BuildServiceProvider(), NullLogger<DespachanteEventos>.Instance);

            // Act
            await despachante.DespacharAsync([_evento]);

            // Assert
            repositorio.Verify(r => r.Adicionar(It.IsAny<EstatisticaTarefa>()), Times.Once);
            outro.Verify(m => m.ManipularAsync(_evento, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
