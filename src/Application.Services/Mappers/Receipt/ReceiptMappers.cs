using Domain.Entities;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Contracts.V1.Entities;
using SpendManagement.Contracts.V1.Events.ReceiptEvents;

namespace Application.Services.Mappers.Receipt
{
    public static class ReceiptMappers
    {
        public static SpendManagementCommand ToCommandDomain(this CreateReceiptCommand createReceiptCommand)
        {
            return new SpendManagementCommand(createReceiptCommand.RoutingKey,
                createReceiptCommand.CommandCreatedDate,
                nameof(CreateReceiptCommand),
                JsonConvert.SerializeObject(createReceiptCommand));
        }

        public static SpendManagementCommand ToCommandDomain(this UpdateReceiptCommand updateReceiptCommand)
        {
            return new SpendManagementCommand(updateReceiptCommand.RoutingKey,
                updateReceiptCommand.CommandCreatedDate,
                nameof(UpdateReceiptCommand),
                JsonConvert.SerializeObject(updateReceiptCommand));
        }

        public static SpendManagementCommand ToCommandDomain(this DeleteReceiptCommand deleteReceiptCommand)
        {
            return new SpendManagementCommand(deleteReceiptCommand.RoutingKey,
                deleteReceiptCommand.CommandCreatedDate,
                nameof(DeleteReceiptCommand),
                JsonConvert.SerializeObject(deleteReceiptCommand));
        }

        public static SpendManagementEvent ToEventDomain(this CreatedReceiptEvent createReceiptEvent)
        {
            return new SpendManagementEvent(
                createReceiptEvent.RoutingKey,
                createReceiptEvent.EventCreatedDate,
                nameof(CreatedReceiptEvent),
                JsonConvert.SerializeObject(createReceiptEvent));
        }

        public static SpendManagementEvent ToEventDomain(this UpdateReceiptEvent updateReceiptEvent)
        {
            return new SpendManagementEvent(
                updateReceiptEvent.RoutingKey,
                updateReceiptEvent.EventCreatedDate,
                nameof(UpdateReceiptEvent),
                JsonConvert.SerializeObject(updateReceiptEvent));
        }

        public static SpendManagementEvent ToEventDomain(this DeleteReceiptEvent deleteReceiptEvent)
        {
            return new SpendManagementEvent(
                deleteReceiptEvent.RoutingKey,
                deleteReceiptEvent.EventCreatedDate,
                nameof(DeleteReceiptEvent),
                JsonConvert.SerializeObject(deleteReceiptEvent));
        }

        public static ReceiptEntity ToDomainEntity(this CreateReceiptCommand receiptCreatedCommand)
        {
            var receiptItems = receiptCreatedCommand.ReceiptItems.Select(x => new ReceiptItemEntity
            {
                Id = x.Id,
                ItemName = x.ItemName,
                ItemPrice = x.ItemPrice,
                Observation = x.Observation,
                Quantity = x.Quantity,
                TotalPrice = x.TotalPrice,
                ItemDiscount = x.ItemDiscount
            });

            return new ReceiptEntity(
                receiptCreatedCommand.Receipt.Id,
                receiptCreatedCommand.Receipt.CategoryId,
                receiptCreatedCommand.Receipt.EstablishmentName,
                receiptCreatedCommand.Receipt.ReceiptDate,
                receiptItems,
                receiptCreatedCommand.Receipt.Discount,
                receiptCreatedCommand.Receipt.Total);
        }

        public static ReceiptEntity ToDomainEntity(this UpdateReceiptCommand updateReceiptCommand)
        {
            var updatedReceiptItems = updateReceiptCommand.ReceiptItems.Select(x => new ReceiptItemEntity
            {
                Id = x.Id,
                ItemName = x.ItemName,
                ItemPrice = x.ItemPrice,
                Observation = x.Observation,
                Quantity = x.Quantity,
                TotalPrice = x.TotalPrice,
                ItemDiscount = x.ItemDiscount
            });

            return new ReceiptEntity(
                updateReceiptCommand.Receipt.Id,
                updateReceiptCommand.Receipt.CategoryId,
                updateReceiptCommand.Receipt.EstablishmentName,
                updateReceiptCommand.Receipt.ReceiptDate,
                updatedReceiptItems,
                updateReceiptCommand.Receipt.Discount,
                updateReceiptCommand.Receipt.Total);
        }

        public static CreatedReceiptEvent ToReceiptCreatedEvent(this CreateReceiptCommand createReceiptCommand)
        {
            var receipt = new SpendManagement.Contracts.V1.Entities.Receipt(createReceiptCommand.Receipt.Id,
                createReceiptCommand.Receipt.CategoryId,
                createReceiptCommand.Receipt.EstablishmentName,
                createReceiptCommand.Receipt.ReceiptDate,
                createReceiptCommand.Receipt.Discount,
                createReceiptCommand.Receipt.Total);

            var receiptItem = createReceiptCommand.ReceiptItems.Select(x => new ReceiptItem(x.Id, x.ItemName, x.Quantity, x.ItemPrice, x.Observation, x.ItemDiscount, x.TotalPrice));
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

            var receiptItems = updateReceiptCommand.ReceiptItems.Select(x => new ReceiptItem(x.Id, x.ItemName, x.Quantity, x.ItemPrice, x.Observation, x.ItemDiscount, x.TotalPrice));
            return new UpdateReceiptEvent(receipt, receiptItems);
        }


        public static DeleteReceiptEvent ToDeleteReceiptEvent(this DeleteReceiptCommand deleteReceiptCommand)
        {
            return new DeleteReceiptEvent(deleteReceiptCommand.Id);
        }
    }
}
