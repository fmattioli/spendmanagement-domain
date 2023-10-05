using Application.Kafka.Events.Interfaces;
using KafkaFlow;
using Serilog;
using SpendManagement.Contracts.V1.Interfaces;

namespace Application.Kafka.Events
{
    public class EventProducer : IEventProducer
    {
        private readonly IMessageProducer<IEvent> eventsProducer;
        private readonly ILogger _logger;

        public EventProducer(IMessageProducer<IEvent> eventProducer, ILogger log)
        {
            this.eventsProducer = eventProducer;
            this._logger = log;
        }

        public async Task SendEventAsync(IEvent spendManagementEvent)
        {
            await eventsProducer.ProduceAsync(spendManagementEvent.RoutingKey, spendManagementEvent);

            _logger.Information("Event produced with success. Event details: {@spendManagementEvent}", spendManagementEvent);
        }
    }
}
