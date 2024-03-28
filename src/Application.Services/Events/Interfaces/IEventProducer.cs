using SpendManagement.Contracts.V1.Interfaces;

namespace Application.Services.Events.Interfaces
{
    public interface IEventProducer
    {
        Task SendEventAsync(IEvent @event);
    }
}
