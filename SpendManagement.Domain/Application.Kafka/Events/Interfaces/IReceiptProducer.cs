using SpendManagement.Contracts.V1.Interfaces;

namespace Application.Kafka.Events.Interfaces
{
    public interface IReceiptProducer
    {
        Task SendEventAsync(IEvent @event);
    }
}
