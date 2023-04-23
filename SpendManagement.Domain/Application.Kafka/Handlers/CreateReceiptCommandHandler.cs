using KafkaFlow;
using KafkaFlow.TypedHandler;
using Serilog;
using SpendManagement.Contracts.V1.Commands;

namespace Application.Kafka.Handlers
{
    public class CreateReceiptCommandHandler : IMessageHandler<CreateReceiptCommand>
    {
        private readonly ILogger log;
        public CreateReceiptCommandHandler(ILogger log)
        {
            this.log = log;
        }

        public async Task Handle(IMessageContext context, CreateReceiptCommand message)
        {
            throw new NotImplementedException();
        }
    }
}
