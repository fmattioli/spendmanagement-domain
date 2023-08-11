using Domain.Entities;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Contracts.V1.Entities;
using SpendManagement.Contracts.V1.Events.ReceiptEvents;

namespace Application.Kafka.Mappers.Receipt
{
    public static class EventMappers
    {
        public static CreatedReceiptEvent ToReceiptCreatedEvent(this CreateReceiptCommand createReceiptCommand)
        {
            var receipt = new SpendManagement.Contracts.V1.Entities.Receipt(createReceiptCommand.Receipt.Id, createReceiptCommand.Receipt.EstablishmentName, createReceiptCommand.Receipt.ReceiptDate);
            var receiptItem = createReceiptCommand.ReceiptItems.Select(x => new ReceiptItem(x.Id, x.ItemName, x.CategoryId, x.Quantity, x.ItemPrice, x.Observation));
            return new CreatedReceiptEvent(createReceiptCommand.RoutingKey, receipt, receiptItem);
        }

        public static Event ToDomain(this CreatedReceiptEvent createReceiptEvent, int commandId)
        {
            return new Event(commandId,
                createReceiptEvent.RoutingKey,
                createReceiptEvent.EventCreatedDate,
                nameof(CreatedReceiptEvent),
                JsonConvert.SerializeObject(createReceiptEvent));
        }

        public static UpdateReceiptEvent ToUpdateReceiptEvent(this UpdateReceiptCommand updateReceiptCommand)
        {
            var receipt = new SpendManagement.Contracts.V1.Entities.Receipt(updateReceiptCommand.Receipt.Id, updateReceiptCommand.Receipt.EstablishmentName, updateReceiptCommand.Receipt.ReceiptDate);
            var receiptItems = updateReceiptCommand.ReceiptItems.Select(x => new ReceiptItem(x.Id, x.ItemName, x.CategoryId, x.Quantity, x.ItemPrice, x.Observation));
            return new UpdateReceiptEvent(updateReceiptCommand.RoutingKey, receipt, receiptItems);
        }

        public static Event ToDomain(this UpdateReceiptEvent updateReceiptEvent, int commandId)
        {
            return new Event(commandId,
                updateReceiptEvent.RoutingKey,
                updateReceiptEvent.EventCreatedDate,
                nameof(UpdateReceiptEvent),
                JsonConvert.SerializeObject(updateReceiptEvent));
        }

        public static DeleteReceiptEvent ToDeleteReceiptEvent(this DeleteReceiptCommand deleteReceiptCommand)
        {
            return new DeleteReceiptEvent(deleteReceiptCommand.RoutingKey, deleteReceiptCommand.Id);
        }

        public static Event ToDomain(this DeleteReceiptEvent deleteReceiptEvent, int commandId)
        {
            return new Event(commandId, 
                deleteReceiptEvent.RoutingKey,
                deleteReceiptEvent.EventCreatedDate,
                nameof(DeleteReceiptEvent),
                JsonConvert.SerializeObject(deleteReceiptEvent));
        }
    }
}
