using Application.Kafka.Events.Interfaces;
using KafkaFlow;
using SpendManagement.Contracts.V1.Interfaces;

namespace Application.Kafka.Events
{
    public class ReceiptProducer : IEventProducer
    {
        private readonly IMessageProducer<IEvent> eventsProducer;

        public ReceiptProducer(IMessageProducer<IEvent> eventProducer)
        {
            this.eventsProducer = eventProducer;
        }

        public async Task SendEventAsync(IEvent @event)
        {
            await eventsProducer.ProduceAsync(@event.RoutingKey, @event);
        }
    }
}
