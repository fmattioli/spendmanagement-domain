using SpendManagement.Contracts.V1.Events.Interfaces;

namespace Application.Kafka.Events.Interfaces
{
    public interface IReceiptProducer
    {
        Task ProduceEvent(IEvent @event);
    }
}
