using ListaDeTarefa.Infrastructure.Jobs;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ListaDeTarefa.Test.Unit.Jobs
{
    public class BaseJobTests
    {
        private class JobDeTeste(ILogger logger, Func<CancellationToken, Task> acao) : BaseJob(logger)
        {
            public override string Nome => "JobDeTeste";

            protected override Task ExecutarJobAsync(CancellationToken cancellationToken) => acao(cancellationToken);
        }

        private static void VerificarLog(Mock<ILogger> logger, LogLevel nivel, Times vezes)
        {
            logger.Verify(l => l.Log(
                nivel,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()), vezes);
        }

        [Fact]
        public async Task ExecutarAsync_DeveExecutarALogicaDoJob()
        {
            var logger = new Mock<ILogger>();
            var executou = false;
            var job = new JobDeTeste(logger.Object, _ => { executou = true; return Task.CompletedTask; });

            await job.ExecutarAsync(CancellationToken.None);

            Assert.True(executou);
            VerificarLog(logger, LogLevel.Information, Times.Exactly(2));
        }

        [Fact]
        public async Task ExecutarAsync_NaoDeveLancarExcecao_QuandoOJobFalha()
        {
            var logger = new Mock<ILogger>();
            var job = new JobDeTeste(logger.Object, _ => throw new InvalidOperationException("falha"));

            var excecao = await Record.ExceptionAsync(() => job.ExecutarAsync(CancellationToken.None));

            Assert.Null(excecao);
            VerificarLog(logger, LogLevel.Error, Times.Once());
        }

        [Fact]
        public async Task ExecutarAsync_NaoDeveLancarExcecao_QuandoCancelado()
        {
            var logger = new Mock<ILogger>();
            using var cts = new CancellationTokenSource();
            cts.Cancel();
            var job = new JobDeTeste(logger.Object, ct => Task.FromCanceled(ct));

            var excecao = await Record.ExceptionAsync(() => job.ExecutarAsync(cts.Token));

            Assert.Null(excecao);
            VerificarLog(logger, LogLevel.Warning, Times.Once());
            VerificarLog(logger, LogLevel.Error, Times.Never());
        }
    }
}
