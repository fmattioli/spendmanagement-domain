using Crosscutting.Models;
using KafkaFlow;
using Polly;
using Serilog;

namespace Crosscutting.Middlewares
{
    public class ProducerRetryMiddleware : IMessageMiddleware
    {
        private readonly int retryCount;
        private readonly TimeSpan retryInterval;
        private readonly ILogger _logger;

        public ProducerRetryMiddleware(ISettings settings, ILogger log)
        {
            if (settings.KafkaSettings is not null)
            {
                this.retryCount = settings.KafkaSettings.ProducerRetryCount;
                this.retryInterval = TimeSpan.FromSeconds(settings.KafkaSettings.ProducerRetryInterval);
            }

            _logger = log;
        }

        public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
        {
            var policyResult = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    this.retryCount,
                    _ => this.retryInterval,
                    (ex, _, __) => _logger.Information("Event produced with success. Event details: {@spendManagementEvent}", ex.Message))
                .ExecuteAndCaptureAsync(() => next(context));
        }
    }
}