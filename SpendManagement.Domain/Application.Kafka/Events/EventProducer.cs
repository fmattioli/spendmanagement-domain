using Application.Kafka.Events.Interfaces;
using KafkaFlow;
using Serilog;
using SpendManagement.Contracts.V1.Interfaces;

namespace Application.Kafka.Events
{
    public class EventProducer : IEventProducer
    {
        private readonly IMessageProducer<IEvent> eventsProducer;
        private readonly ILogger _log;

        public EventProducer(IMessageProducer<IEvent> eventProducer, ILogger log)
        {
            this.eventsProducer = eventProducer;
            this._log = log;
        }

        public async Task SendEventAsync(IEvent @event)
        {
            await eventsProducer.ProduceAsync(@event.RoutingKey, @event);
        }
    }
}
