using Domain.Entities;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;

namespace Application.Kafka.Mappers.Receipt
{
    public static class CommandMappers
    {
        public static Command ToDomain(this CreateReceiptCommand createReceiptCommand)
        {
            return new Command(createReceiptCommand.RoutingKey,
                createReceiptCommand.CommandCreatedDate,
                nameof(CreateReceiptCommand),
                JsonConvert.SerializeObject(createReceiptCommand));
        }

        public static Command ToDomain(this UpdateReceiptCommand updateReceiptCommand)
        {
            return new Command(updateReceiptCommand.RoutingKey,
                updateReceiptCommand.CommandCreatedDate,
                nameof(UpdateReceiptCommand),
                JsonConvert.SerializeObject(updateReceiptCommand));
        }

        public static Command ToDomain(this DeleteReceiptCommand deleteReceiptCommand)
        {
            return new Command(deleteReceiptCommand.RoutingKey,
                deleteReceiptCommand.CommandCreatedDate,
                nameof(DeleteReceiptCommand),
                JsonConvert.SerializeObject(deleteReceiptCommand));
        }
    }
}
