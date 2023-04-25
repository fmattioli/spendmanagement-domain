using Application.Kafka.Events.Interfaces;
using KafkaFlow;
using SpendManagement.Contracts.V1.Events.Interfaces;
using SpendManagement.Topics;

namespace Application.Kafka.Events
{
    public class ReceiptProducer : IReceiptProducer
    {
        private readonly IMessageProducer<IEvent> eventsProducer;

        public ReceiptProducer(IMessageProducer<IEvent> eventProducer)
        {
            this.eventsProducer = eventProducer;
        }

        public async Task ProduceEvent(IEvent @event)
        {
            await eventsProducer.ProduceAsync(KafkaTopics.Events.ReceiptEventTopicName, @event.RoutingKey, @event);
        }
    }
}
