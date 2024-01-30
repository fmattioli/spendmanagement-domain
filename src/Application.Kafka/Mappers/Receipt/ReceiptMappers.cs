using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Contracts.V1.Entities;
using SpendManagement.Contracts.V1.Events.ReceiptEvents;

namespace Application.Kafka.Mappers.Receipt
{
    public static class ReceiptMappers
    {
        public static CreatedReceiptEvent ToReceiptCreatedEvent(this CreateReceiptCommand createReceiptCommand)
        {
            var receipt = new SpendManagement.Contracts.V1.Entities.Receipt(createReceiptCommand.Receipt.Id,
                createReceiptCommand.Receipt.CategoryId,
                createReceiptCommand.Receipt.EstablishmentName,
                createReceiptCommand.Receipt.ReceiptDate,
                createReceiptCommand.Receipt.Discount,
                createReceiptCommand.Receipt.Total);

            var receiptItem = createReceiptCommand.ReceiptItems.Select(x => new ReceiptItem(x.Id, x.ItemName, x.Quantity, x.ItemPrice, x.Observation, x.ItemDiscount));
            return new CreatedReceiptEvent(receipt, receiptItem);
        }

        public static UpdateReceiptEvent ToUpdateReceiptEvent(this UpdateReceiptCommand updateReceiptCommand)
        {
            var receipt = new SpendManagement.Contracts.V1.Entities.Receipt(updateReceiptCommand.Receipt.Id,
                updateReceiptCommand.Receipt.CategoryId,
                updateReceiptCommand.Receipt.EstablishmentName,
                updateReceiptCommand.Receipt.ReceiptDate,
                updateReceiptCommand.Receipt.Discount,
                updateReceiptCommand.Receipt.Total);

            var receiptItems = updateReceiptCommand.ReceiptItems.Select(x => new ReceiptItem(x.Id, x.ItemName, x.Quantity, x.ItemPrice, x.Observation, x.ItemDiscount));
            return new UpdateReceiptEvent(receipt, receiptItems);
        }


        public static DeleteReceiptEvent ToDeleteReceiptEvent(this DeleteReceiptCommand deleteReceiptCommand)
        {
            return new DeleteReceiptEvent(deleteReceiptCommand.Id);
        }

    }
}
