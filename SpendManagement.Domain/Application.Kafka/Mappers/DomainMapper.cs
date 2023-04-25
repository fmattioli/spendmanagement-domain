using Domain.Entities;

using SpendManagement.Contracts.V1.Base;
using SpendManagement.Contracts.V1.Commands;
using SpendManagement.Contracts.V1.Events;

namespace Application.Kafka.Converters
{
    public static class Mappers
    {
        public static Receipt ToDomain(this CreateReceiptCommand createReceiptCommand)
        {
            return new Receipt(
                createReceiptCommand.Id,
                createReceiptCommand.EstablishmentName,
                createReceiptCommand.ReceiptDate,
                createReceiptCommand.ReceiptItems.Select(x => new Domain.ValueObjects.ReceiptItem
                {
                    Id = x.Id,
                    ItemName = x.ItemName,
                    ItemPrice = x.ItemPrice,
                    Observation = x.Observation,
                    Quantity = x.Quantity
                })
            );
        }

        public static ReceiptCreatedEvent ToReceiptCreatedEvent(this Receipt receipt)
        {
            return new ReceiptCreatedEvent
            {
                Id = receipt.Id,
                EstablishmentName = receipt.EstablishmentName,
                ReceiptDate = receipt.ReceiptDate,
                ReceiptItems = receipt.ReceiptItems.Select(x => new ReceiptItem
                {
                    Id = x.Id,
                    ItemName = x.ItemName,
                    ItemPrice = x.ItemPrice,
                    Observation = x.Observation,
                    Quantity = x.Quantity
                })
            };
        }
    }
}
