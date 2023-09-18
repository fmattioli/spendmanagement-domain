﻿using Crosscutting.Models;
using KafkaFlow;
using Polly;

namespace Crosscutting.Middlewares
{
    public class ProducerRetryMiddleware : IMessageMiddleware
    {
        private readonly int retryCount;
        private readonly TimeSpan retryInterval;
        public ProducerRetryMiddleware(ISettings settings)
        {
            if (settings.KafkaSettings is not null)
            {
                this.retryCount = settings.KafkaSettings.ProducerRetryCount;
                this.retryInterval = TimeSpan.FromSeconds(settings.KafkaSettings.ProducerRetryInterval);
            }
        }

        public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
        {
            var policyResult = await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    this.retryCount,
                    _ => this.retryInterval,
                    (ex, _, __) =>
                    {
                        Console.WriteLine(ex);
                    })
                .ExecuteAndCaptureAsync(() => next(context));

            //TODO: Generate Log wth Microsoft.Logging
        }
    }
}