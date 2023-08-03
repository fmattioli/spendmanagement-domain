using Domain.Entities;
using Newtonsoft.Json;

using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;

namespace Application.Kafka.Converters
{
    public static class CommandMapper
    {
        public static Command ToDomain(this CreateReceiptCommand createReceiptCommand)
        {
            return new Command(createReceiptCommand.RoutingKey,
                createReceiptCommand.CommandCreatedDate,
                nameof(CreateReceiptCommand),
                JsonConvert.SerializeObject(createReceiptCommand));
        }

        public static Command ToDomain(this CreateCategoryCommand createCategoryCommand)
        {
            return new Command(createCategoryCommand.RoutingKey,
                createCategoryCommand.CommandCreatedDate,
                nameof(CreateCategoryCommand),
                JsonConvert.SerializeObject(createCategoryCommand));
        }
    }
}
