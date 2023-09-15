using Application.Kafka.Constants;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Crosscutting.Extensions
{
    public static class TracingExtensions
    {
        public static IServiceCollection AddTracing(this IServiceCollection services)
        {
            services.AddOpenTelemetry().WithTracing(tcb =>
            {
                tcb
                .AddSource(Constants.ApplicationName)
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName: Constants.ApplicationName))
                .AddSqlClientInstrumentation(options => options.SetDbStatementForText = true)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter();
            });

            services.AddSingleton(TracerProvider.Default.GetTracer(Constants.ApplicationName));

            return services;
        }
    }
}
