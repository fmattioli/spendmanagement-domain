﻿using Application.Kafka.Events;
using Application.Kafka.Events.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.Extensions
{
    public static class ProducerExtensions
    {
        public static IServiceCollection AddServiceEventsProducer(this IServiceCollection services)
        {
            services.AddScoped<IEventProducer, EventProducer>();
            return services;
        }
    }
}
