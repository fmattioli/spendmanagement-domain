using KafkaFlow;
using KafkaFlow.TypedHandler;

using Spents.Contracts.V1.Commands;

namespace Application.Kafka.Handlers
{
    public class CreateReceiptCommandHandler : IMessageHandler<CreateReceiptCommand>
    {
        public async Task Handle(IMessageContext context, CreateReceiptCommand message)
        {
            throw new NotImplementedException();
        }
    }
}
