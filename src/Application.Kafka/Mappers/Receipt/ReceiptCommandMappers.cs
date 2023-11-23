using Domain.Entities;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;

namespace Application.Kafka.Mappers.Receipt
{
    public static class ReceiptCommandMappers
    {
        public static SpendManagementCommand ToDomain(this CreateReceiptCommand createReceiptCommand)
        {
            return new SpendManagementCommand(createReceiptCommand.RoutingKey,
                createReceiptCommand.CommandCreatedDate,
                nameof(CreateReceiptCommand),
                JsonConvert.SerializeObject(createReceiptCommand));
        }

        public static SpendManagementCommand ToDomain(this UpdateReceiptCommand updateReceiptCommand)
        {
            return new SpendManagementCommand(updateReceiptCommand.RoutingKey,
                updateReceiptCommand.CommandCreatedDate,
                nameof(UpdateReceiptCommand),
                JsonConvert.SerializeObject(updateReceiptCommand));
        }

        public static SpendManagementCommand ToDomain(this DeleteReceiptCommand deleteReceiptCommand)
        {
            return new SpendManagementCommand(deleteReceiptCommand.RoutingKey,
                deleteReceiptCommand.CommandCreatedDate,
                nameof(DeleteReceiptCommand),
                JsonConvert.SerializeObject(deleteReceiptCommand));
        }
    }
}
