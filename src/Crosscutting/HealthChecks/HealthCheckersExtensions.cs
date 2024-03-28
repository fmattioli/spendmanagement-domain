using Confluent.Kafka;
using Crosscutting.Models;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.HealthChecks
{
    public static class HealthCheckersExtensions
    {
        private static readonly string[] tags = ["db", "data"];

        public static IServiceCollection AddHealthCheckers(this IServiceCollection services, Settings settings)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = settings.KafkaSettings.Sasl_Brokers.First(),
                SaslMechanism = SaslMechanism.ScramSha256,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = settings.KafkaSettings.Sasl_Username,
                SaslPassword = settings.KafkaSettings.Sasl_Password,
            };

            services
                .AddHealthChecks()
                .AddKafka(producerConfig, name: "Kafka")
                .AddNpgSql(settings.SqlSettings.ConnectionString, name: "Postgres", tags: tags);

            services
                .AddHealthChecksUI(setupSettings: setup => setup.SetEvaluationTimeInSeconds(60))
                .AddInMemoryStorage();
            return services;
        }

        public static void UseHealthCheckers(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options => options.UIPath = "/monitor");
        }
    }
}
