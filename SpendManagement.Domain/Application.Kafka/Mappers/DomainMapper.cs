using Domain.Entities;
using Domain.ValueObjects;
using SpendManagement.Contracts.V1.Base;
using SpendManagement.Contracts.V1.Commands;
using SpendManagement.Contracts.V1.Events;
using SpendManagement.Contracts.V1.Entities;

namespace Application.Kafka.Converters
{
    public static class Mappers
    {
        public static ReceiptDomain ToDomain(this CreateReceiptCommand createReceiptCommand)
        {
            return new ReceiptDomain(createReceiptCommand.Receipt.Id,
                createReceiptCommand.Receipt.EstablishmentName,
                createReceiptCommand.Receipt.ReceiptDate,
                createReceiptCommand.ReceiptItems.Select(x => new ReceiptItemDomain
                {
                    Id = x.Id,
                    Category = new CategoryDomain(x.Category.Id, x.Category.Name),
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
                ReceiptItem = receipt.ReceiptItems.Select(x => new ReceiptItem(x.Id, x.ItemName, new Category(x.Category.Id, x.Category.Name), x.Quantity, x.ItemPrice, x.Observation))
            };
        }
    }
}
