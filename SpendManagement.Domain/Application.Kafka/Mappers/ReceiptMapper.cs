using Domain.Entities;
using Domain.ValueObjects;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Contracts.V1.Entities;
using SpendManagement.Contracts.V1.Events.ReceiptEvents;

namespace Application.Kafka.Converters
{
    public static class ReceiptMappers
    {
        public static ReceiptDomain ToDomain(this CreateReceiptCommand createReceiptCommand)
        {
            return new ReceiptDomain(createReceiptCommand.Receipt.Id,
                createReceiptCommand.Receipt.EstablishmentName,
                createReceiptCommand.Receipt.ReceiptDate,
                createReceiptCommand.ReceiptItems.Select(x => new ReceiptItemDomain
                {
                    Id = x.Id,
                    CategoryId =x.CategoryId,
                    ItemName = x.ItemName,
                    ItemPrice = x.ItemPrice,
                    Observation = x.Observation,
                    Quantity = x.Quantity,
                    ReceiptId = createReceiptCommand.Receipt.Id
                }));
        }

        public static CreatedReceiptEvent ToReceiptCreatedEvent(this ReceiptDomain receipt)
        {
            return new CreatedReceiptEvent
            {
                Receipt = new Receipt(receipt.Id, receipt.EstablishmentName, receipt.ReceiptDate),
                ReceiptItem = receipt.ReceiptItems.Select(x => new ReceiptItem(x.Id, x.ItemName, x.CategoryId, x.Quantity, x.ItemPrice, x.Observation))
            };
        }
    }
}
