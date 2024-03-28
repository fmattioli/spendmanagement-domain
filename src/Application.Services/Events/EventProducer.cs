using Application.Services.Events.Interfaces;
using KafkaFlow;
using Serilog;
using SpendManagementEvents = SpendManagement.Contracts.V1.Interfaces;

namespace Application.Services.Events
{
    public class EventProducer : IEventProducer
    {
        private readonly IMessageProducer<SpendManagementEvents.IEvent> eventsProducer;
        private readonly ILogger _logger;

        public EventProducer(IMessageProducer<SpendManagementEvents.IEvent> eventProducer, ILogger log)
        {
            this.eventsProducer = eventProducer;
            this._logger = log;
        }

        public async Task SendEventAsync(SpendManagementEvents.IEvent spendManagementEvent)
        {
            await eventsProducer.ProduceAsync(spendManagementEvent.RoutingKey, spendManagementEvent);

            _logger.Information("Event produced with success. Event details: {@spendManagementEvent}", spendManagementEvent);
        }
    }
}
