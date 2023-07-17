using KafkaFlow;
using KafkaFlow.TypedHandler;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;

namespace Application.Kafka.Commands.Handlers
{
    public class CategoryCommandHandler : IMessageHandler<CreateCategoryCommand>
    {
        public Task Handle(IMessageContext context, CreateCategoryCommand message)
        {
            throw new NotImplementedException();
        }
    }
}
