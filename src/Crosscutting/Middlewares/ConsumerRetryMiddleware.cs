using Crosscutting.Extensions;
using Crosscutting.Models;
using KafkaFlow;
using Polly;
using Serilog;

namespace Crosscutting.Middlewares
{
    public class ConsumerRetryMiddleware : IMessageMiddleware
    {
        private readonly ILogger log;

        private readonly int retryCount;

        private readonly TimeSpan retryInterval;

        public ConsumerRetryMiddleware(ILogger log, ISettings settings)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.retryCount = settings.KafkaSettings.ConsumerRetryCount;
            this.retryInterval = TimeSpan.FromSeconds(settings.KafkaSettings.ConsumerRetryInterval);
        }

        public Task Invoke(IMessageContext context, MiddlewareDelegate next)
        {
            return Policy
                .Handle<Exception>(ex => ex is not InvalidOperationException)
                .WaitAndRetryAsync(
                    this.retryCount,
                    _ => this.retryInterval,
                    (ex, _, retryAttempt, __) =>
                    {
                        this.log.Warning(
                            $"[{nameof(ConsumerRetryMiddleware)}] - Failed to process message, retrying... " + ex.Message,
                            ex,
                            () => new
                            {
                                context.ConsumerContext.Topic,
                                context.ConsumerContext.Offset,
                                PartitionNumber = context.ConsumerContext.Partition,
                                PartitionKey = context.GetPartitionKey(),
                                Headers = context.Headers.ToJsonString(),
                                RetryAttempt = retryAttempt,
                                ex.Message
                            });
                    })
                .ExecuteAsync(() => next(context));
        }
    }
}
