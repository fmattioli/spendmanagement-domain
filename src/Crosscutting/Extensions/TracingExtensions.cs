using Application.Services.Constants;
using Crosscutting.Models;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Crosscutting.Extensions
{
    public static class TracingExtensions
    {
        public static IServiceCollection AddTracing(this IServiceCollection services, TracingSettings? tracing)
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
                .AddOtlpExporter(opt =>
                {
                    opt.Endpoint = new Uri(tracing?.Uri + ":" + tracing?.Port);
                    opt.Protocol = OtlpExportProtocol.Grpc;
                });
            });

            services.AddSingleton(TracerProvider.Default.GetTracer(Constants.ApplicationName));

            return services;
        }
    }
}
