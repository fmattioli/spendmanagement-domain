using Crosscutting.Middlewares;
using Crosscutting.Models;
using KafkaFlow;
using KafkaFlow.Serializer;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Json;
using SpendManagement.Contracts.V1.Interfaces;
using SpendManagement.Domain.Integration.Tests.Configuration;
using SpendManagement.Domain.Integration.Tests.Helpers;
using SpendManagement.Topics;

namespace SpendManagement.Domain.Integration.Tests.Fixtures
{
    public class KafkaFixture : IAsyncLifetime
    {
        private readonly IKafkaBus _bus;
        private readonly IMessageProducer<ICommand> _messageProducer;
        private readonly KafkaMessageHelper kafkaMessage = new();

        public KafkaFixture()
        {
            var settings = TestSettings.Kafka;

            var services = new ServiceCollection();

            services.AddSingleton<ISettings>(new Settings
            {
                KafkaSettings = new Crosscutting.Models.KafkaSettings
                {
                    Environment = "tests",
                    ProducerRetryCount = 1,
                    ConsumerRetryInterval = 100,
                    ConsumerInitialState = "Running",
                    MessageTimeoutMs = 45000,
                    ConsumerRetryCount = 1,
                    WorkerCount = 2,
                    BufferSize = 4
                }
            });

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(new JsonFormatter())
                .CreateLogger();

            services.AddSingleton(Log.Logger);

            services.AddKafka(kafka => kafka
               .UseLogHandler<ConsoleLogHandler>()
                   .AddCluster(cluster => cluster
                       .WithBrokers(settings?.Brokers)
                       .AddConsumer(consumer =>
                       {
                           consumer
                               .Topics(KafkaTopics.Events.GetReceiptEvents(settings!.Environment))
                               .WithGroupId("Receipts-Events")
                               .WithName("Receipt-Events")
                               .WithBufferSize(settings!.Batch!.BufferSize)
                               .WithWorkersCount(settings!.Batch!.WorkerCount)
                               .WithAutoOffsetReset(AutoOffsetReset.Latest)
                               .WithInitialState(Enum.Parse<ConsumerInitialState>("Running"))
                               .AddMiddlewares(middlewares => middlewares
                                   .AddDeserializer<JsonCoreDeserializer>()
                                   .Add(_ => this.kafkaMessage));
                       })
                       .CreateTopicIfNotExists(KafkaTopics.Commands.GetReceiptCommands(settings!.Environment), 2, 1)
                       .AddProducer<ICommand>(
                       p => p
                       .DefaultTopic(KafkaTopics.Commands.GetReceiptCommands(settings!.Environment))
                       .AddMiddlewares(m => m
                        .Add<ProducerRetryMiddleware>()
                        .Add<ProducerTracingMiddleware>()
                        .AddSerializer<JsonCoreSerializer>())
                    .WithAcks(Acks.All)
            )));

            var provider = services.BuildServiceProvider();
            this._bus = provider.CreateKafkaBus();
            this._messageProducer = provider.GetRequiredService<IMessageProducer<SpendManagement.Contracts.V1.Interfaces.ICommand>>();
        }

        public Task DisposeAsync()
        {
            return this._bus.StopAsync();
        }

        public async Task InitializeAsync()
        {
            await this._bus.StartAsync();
            await Task.Delay(TestSettings.Kafka!.InitializationDelay);
        }

        public Task ProduceCommandAsync(ICommand message, IMessageHeaders? headers = null)
        {
            return this._messageProducer.ProduceAsync(message.RoutingKey, message, headers, null);
        }

        public TMessage Consume<TMessage>(Func<TMessage, IMessageHeaders, bool> predicate)
            where TMessage : struct
        {
            return this.kafkaMessage.TryTake(
                predicate,
                300 * 200);
        }
    }
}
