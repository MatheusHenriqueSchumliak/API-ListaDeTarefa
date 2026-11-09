using ListaDeTarefa.Application.Interfaces;
using ListaDeTarefa.Domain.Events;
using ListaDeTarefa.Infrastructure.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Events
{
    public class ProcessadorEventosServiceTests
    {
        private static readonly TarefaAtribuidaEvento _evento = new(Guid.NewGuid(), Guid.NewGuid(), "Tarefa", DateTimeOffset.UtcNow);

        private sealed class ManipuladorSinalizador(TaskCompletionSource<TarefaAtribuidaEvento> sinal) : IManipuladorEvento<TarefaAtribuidaEvento>
        {
            public Task ManipularAsync(TarefaAtribuidaEvento evento, CancellationToken cancellationToken = default)
            {
                sinal.TrySetResult(evento);
                return Task.CompletedTask;
            }
        }

        private static ServiceProvider CriarProvider(FilaEventos fila, IManipuladorEvento<TarefaAtribuidaEvento> manipulador)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(fila);
            services.AddSingleton(manipulador);
            services.AddScoped<DespachanteEventos>();
            return services.BuildServiceProvider();
        }

        [Fact]
        public async Task DespachanteEmFila_NaoDeveExecutarManipuladorNaHora_SoEnfileirar()
        {
            // Arrange
            var fila = new FilaEventos();
            var manipulador = new Mock<IManipuladorEvento<TarefaAtribuidaEvento>>();
            var despachante = new DespachanteEventosEmFila(fila);

            // Act
            await despachante.DespacharAsync([_evento]);

            // Assert
            manipulador.Verify(m => m.ManipularAsync(It.IsAny<TarefaAtribuidaEvento>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Processador_DeveEntregarEventoEnfileiradoAoManipulador()
        {
            // Arrange
            var fila = new FilaEventos();
            var sinal = new TaskCompletionSource<TarefaAtribuidaEvento>(TaskCreationOptions.RunContinuationsAsynchronously);
            using var provider = CriarProvider(fila, new ManipuladorSinalizador(sinal));
            var processador = new ProcessadorEventosService(fila, provider.GetRequiredService<IServiceScopeFactory>(), NullLogger<ProcessadorEventosService>.Instance);
            await processador.StartAsync(CancellationToken.None);

            // Act
            await new DespachanteEventosEmFila(fila).DespacharAsync([_evento]);
            var recebido = await sinal.Task.WaitAsync(TimeSpan.FromSeconds(5));

            // Assert
            Assert.Equal(_evento, recebido);
            await processador.StopAsync(CancellationToken.None);
        }

        [Fact]
        public async Task Processador_DeveContinuarProcessando_QuandoManipuladorFalha()
        {
            // Arrange
            var fila = new FilaEventos();
            var falho = new Mock<IManipuladorEvento<TarefaAtribuidaEvento>>();
            falho.Setup(m => m.ManipularAsync(It.IsAny<TarefaAtribuidaEvento>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("falha"));
            var sinal = new TaskCompletionSource<TarefaAtribuidaEvento>(TaskCreationOptions.RunContinuationsAsynchronously);
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(fila);
            services.AddSingleton(falho.Object);
            services.AddSingleton<IManipuladorEvento<TarefaAtribuidaEvento>>(new ManipuladorSinalizador(sinal));
            services.AddScoped<DespachanteEventos>();
            using var provider = services.BuildServiceProvider();
            var processador = new ProcessadorEventosService(fila, provider.GetRequiredService<IServiceScopeFactory>(), NullLogger<ProcessadorEventosService>.Instance);
            await processador.StartAsync(CancellationToken.None);

            // Act
            await new DespachanteEventosEmFila(fila).DespacharAsync([_evento]);
            var recebido = await sinal.Task.WaitAsync(TimeSpan.FromSeconds(5));

            // Assert
            Assert.Equal(_evento, recebido);
            await processador.StopAsync(CancellationToken.None);
        }
    }
}
