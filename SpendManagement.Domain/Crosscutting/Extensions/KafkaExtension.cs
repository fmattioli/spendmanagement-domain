using KafkaFlow;
using KafkaFlow.Admin.Dashboard;
using KafkaFlow.Configuration;
using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Crosscutting.Models;
using Crosscutting.HostedService;
using SpendManagement.Topics;
using Application.Kafka.Commands.Handlers;
using SpendManagement.Contracts.V1.Events.Interfaces;
using Crosscutting.Middlewares;

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
                    .AddTelemetry()
                    .AddConsumers(kafkaSettings)
                    .AddProducers(kafkaSettings)
                    )
                );
            services.AddHostedService<KafkaBusHostedService>();
            return services;
        }

        private static IClusterConfigurationBuilder AddTelemetry(
            this IClusterConfigurationBuilder builder)
        {
            builder
                .EnableAdminMessages(KafkaTopics.Commands.ReceiptTelemetry)
                .EnableTelemetry(KafkaTopics.Commands.ReceiptTelemetry);

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
                        si.SecurityProtocol = KafkaFlow.Configuration.SecurityProtocol.SaslSsl;
                        si.SaslUsername = settings.Sasl_UserName;
                        si.SaslPassword = settings.Sasl_Password;
                        si.SaslMechanism = KafkaFlow.Configuration.SaslMechanism.Plain;
                        si.SslCaLocation = string.Empty;
                    });
            }
            else
            {
                builder.WithBrokers(new[] { settings?.Brokers });
            }

            return builder;
        }

        private static IClusterConfigurationBuilder AddConsumers(
            this IClusterConfigurationBuilder builder,
            KafkaSettings? settings)
        {
            builder.AddConsumer(
                consumer => consumer
                     .Topics(KafkaTopics.Commands.ReceiptCommandTopicName)
                     .WithGroupId("Receipts-Commands")
                     .WithName("Receipt-Commands")
                     .WithBufferSize(settings?.BufferSize ?? 0)
                     .WithWorkersCount(settings?.WorkerCount ?? 0)
                     .WithAutoOffsetReset(KafkaFlow.AutoOffsetReset.Latest)
                     .AddMiddlewares(
                        middlewares =>
                            middlewares
                            .AddSerializer<JsonCoreSerializer>()
                            .Add<ConsumerLoggingMiddleware>()
                            .Add<ConsumerRetryMiddleware>()
                            .AddTypedHandlers(
                                h => h
                                    .WithHandlerLifetime(InstanceLifetime.Scoped)
                                    .AddHandler<CreateReceiptCommandHandler>()
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

            builder.
                CreateTopicIfNotExists(KafkaTopics.Events.ReceiptEventTopicName, 2, 1)
                .AddProducer<IEvent>(p => p
                .DefaultTopic(KafkaTopics.Events.ReceiptEventTopicName)
                .AddMiddlewares(m => m
                    .Add<ProducerRetryMiddleware>()
                    .AddSerializer<JsonCoreSerializer>())
                .WithAcks(KafkaFlow.Acks.All)
                .WithProducerConfig(producerConfig));

            return builder;
        }
    }
}
