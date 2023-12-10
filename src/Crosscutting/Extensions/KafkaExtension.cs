using Application.Kafka.Commands.Handlers;
using Crosscutting.HostedService;
using Crosscutting.Middlewares;
using Crosscutting.Models;
using KafkaFlow;
using KafkaFlow.Admin.Dashboard;
using KafkaFlow.Configuration;
using KafkaFlow.Serializer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SpendManagement.Topics;

namespace Crosscutting.Extensions
{
    public static class KafkaExtension
    {
        public static IApplicationBuilder ShowKafkaDashboard(this IApplicationBuilder app) => app.UseKafkaFlowDashboard();

        public static IServiceCollection AddKafka(this IServiceCollection services, KafkaSettings kafkaSettings)
        {
            services.AddKafka(kafka => kafka
                .UseConsoleLog()
                .AddCluster(cluster => cluster
                    .AddBrokers(kafkaSettings)
                    .AddTelemetry(kafkaSettings.Environment)
                    .AddConsumers(kafkaSettings)
                    .AddProducers(kafkaSettings)
                    )
                );
            services.AddHostedService<KafkaBusHostedService>();
            return services;
        }

        private static IClusterConfigurationBuilder AddTelemetry(
            this IClusterConfigurationBuilder builder,
            string enviroment)
        {
            var topic = $"{enviroment}.spendmanagement.receipts.events.telemetry";

            builder
                .EnableAdminMessages(topic)
                .EnableTelemetry(topic);

            return builder;
        }

        private static IClusterConfigurationBuilder AddBrokers(
            this IClusterConfigurationBuilder builder,
            KafkaSettings? settings)
        {
            if (settings?.Sasl_Enabled ?? false)
            {
                builder
                    .WithBrokers(settings.Sasl_Brokers)
                    .WithSecurityInformation(si =>
                    {
                        si.SecurityProtocol = SecurityProtocol.SaslSsl;
                        si.SaslUsername = settings.Sasl_UserName;
                        si.SaslPassword = settings.Sasl_Password;
                        si.SaslMechanism = SaslMechanism.Plain;
                        si.SslCaLocation = string.Empty;
                    });
            }
            else
            {
                builder.WithBrokers(new[] { settings?.Broker });
            }

            return builder;
        }

        private static IClusterConfigurationBuilder AddConsumers(
            this IClusterConfigurationBuilder builder,
            KafkaSettings? settings)
        {
            builder.AddConsumer(
                consumer => consumer
                     .Topics(KafkaTopics.Commands.GetReceiptCommands(settings!.Environment))
                     .WithGroupId("Receipts-Commands")
                     .WithName("Receipt-Commands")
                     .WithBufferSize(settings?.BufferSize ?? 0)
                     .WithWorkersCount(settings?.WorkerCount ?? 0)
                     .WithAutoOffsetReset(AutoOffsetReset.Latest)
                     .WithInitialState(Enum.Parse<ConsumerInitialState>(settings?.ConsumerInitialState ?? "Running"))
                     .AddMiddlewares(m => m
                            .Add<ConsumerLoggingMiddleware>()
                            .Add<ConsumerTracingMiddleware>()
                            .AddDeserializer<JsonCoreDeserializer>()
                            .Add<ConsumerRetryMiddleware>()
                            .AddTypedHandlers(
                                h => h
                                    .WithHandlerLifetime(InstanceLifetime.Scoped)
                                    .AddHandlersFromAssemblyOf<ReceiptCommandHandler>()
                                    )
                            )
                     );

            return builder;
        }

        private static IClusterConfigurationBuilder AddProducers(
          this IClusterConfigurationBuilder builder,
          KafkaSettings settings)
        {
            var producerConfig = new Confluent.Kafka.ProducerConfig
            {
                MessageTimeoutMs = settings.MessageTimeoutMs,
            };

            builder
                .AddProducer<SpendManagement.Contracts.V1.Interfaces.IEvent>(
                p => p
                    .DefaultTopic(KafkaTopics.Events.GetReceiptEvents(settings.Environment))
                    .AddMiddlewares(m => m
                        .Add<ProducerRetryMiddleware>()
                        .Add<ProducerTracingMiddleware>()
                        .AddSerializer<JsonCoreSerializer>())
                    .WithAcks(Acks.All)
                    .WithProducerConfig(producerConfig));

            return builder;
        }
    }
}
