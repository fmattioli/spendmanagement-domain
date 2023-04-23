using Domain.Entities;
using SpendManagement.Contracts.V1.Commands;

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
    }
}
