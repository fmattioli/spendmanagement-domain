using Domain.Entities;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Contracts.V1.Entities;
using SpendManagement.Contracts.V1.Events.CategoryEvents;
using SpendManagement.Contracts.V1.Events.ReceiptEvents;

namespace Application.Kafka.Mappers
{
    public static class EventMapper
    {
        public static Event ToDomain(this CreatedReceiptEvent createReceiptEvent)
        {
            return new Event(createReceiptEvent.RoutingKey,
                createReceiptEvent.EventCreatedDate,
                nameof(CreatedReceiptEvent),
                JsonConvert.SerializeObject(createReceiptEvent));
        }

        public static Event ToDomain(this CreateCategoryEvent createCategoryEvent)
        {
            return new Event(createCategoryEvent.RoutingKey,
                createCategoryEvent.EventCreatedDate,
                nameof(CreateCategoryEvent),
                JsonConvert.SerializeObject(createCategoryEvent));
        }

        public static CreatedReceiptEvent ToReceiptCreatedEvent(this CreateReceiptCommand receipt)
        {
            return new CreatedReceiptEvent
            {
                Receipt = new Receipt(receipt.Receipt.Id, receipt.Receipt.EstablishmentName, receipt.Receipt.ReceiptDate),
                ReceiptItem = receipt.ReceiptItems.Select(x => new ReceiptItem(x.Id, x.ItemName, x.CategoryId, x.Quantity, x.ItemPrice, x.Observation)),
                EventCreatedDate = DateTime.UtcNow,
            };
        }

        public static CreateCategoryEvent ToCreateCategoryEvent(this CreateCategoryCommand createCategoryCommand)
        {
            return new CreateCategoryEvent
            {
                Category = new Category(createCategoryCommand.Category.Id, createCategoryCommand.Category.Name),
                EventCreatedDate = DateTime.UtcNow,
            };
        }
    }
}
