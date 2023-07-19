using SpendManagement.Contracts.V1.Interfaces;

namespace Application.Kafka.Events.Interfaces
{
    public interface IEventProducer
    {
        Task SendEventAsync(IEvent @event);
    }
}
