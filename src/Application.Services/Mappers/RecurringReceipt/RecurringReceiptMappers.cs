using Domain.Entities;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.RecurringReceiptCommands;
using SpendManagement.Contracts.V1.Events.RecurringReceiptEvents;

namespace Application.Services.Mappers.RecurringReceipt
{
    public static class RecurringReceiptMappers
    {
        public static SpendManagementCommand ToCommandDomain(this CreateRecurringReceiptCommand createReceiptCommand)
        {
            return new SpendManagementCommand(createReceiptCommand.RoutingKey,
                createReceiptCommand.CommandCreatedDate,
                nameof(CreateRecurringReceiptCommand),
                JsonConvert.SerializeObject(createReceiptCommand));
        }

        public static SpendManagementCommand ToCommandDomain(this UpdateRecurringReceiptCommand updateReceiptCommand)
        {
            return new SpendManagementCommand(updateReceiptCommand.RoutingKey,
                updateReceiptCommand.CommandCreatedDate,
                nameof(UpdateRecurringReceiptCommand),
                JsonConvert.SerializeObject(updateReceiptCommand));
        }

        public static SpendManagementCommand ToCommandDomain(this DeleteRecurringReceiptCommand deleteReceiptCommand)
        {
            return new SpendManagementCommand(deleteReceiptCommand.RoutingKey,
                deleteReceiptCommand.CommandCreatedDate,
                nameof(DeleteRecurringReceiptCommand),
                JsonConvert.SerializeObject(deleteReceiptCommand));
        }

        public static RecurringReceiptEntity ToDomainEntity(this CreateRecurringReceiptCommand recurringReceiptCreatedEvent)
        {
            return new RecurringReceiptEntity(
                recurringReceiptCreatedEvent.RecurringReceipt.Id,
                recurringReceiptCreatedEvent.RecurringReceipt.CategoryId,
                recurringReceiptCreatedEvent.RecurringReceipt.EstablishmentName,
                recurringReceiptCreatedEvent.RecurringReceipt.DateInitialRecurrence,
                recurringReceiptCreatedEvent.RecurringReceipt.DateEndRecurrence,
                recurringReceiptCreatedEvent.RecurringReceipt.RecurrenceTotalPrice,
                recurringReceiptCreatedEvent.RecurringReceipt.Observation);
        }

        public static RecurringReceiptEntity ToDomainEntity(this UpdateRecurringReceiptCommand updateRecurringReceiptEvent)
        {
            return new RecurringReceiptEntity(
                updateRecurringReceiptEvent.RecurringReceipt.Id,
                updateRecurringReceiptEvent.RecurringReceipt.CategoryId,
                updateRecurringReceiptEvent.RecurringReceipt.EstablishmentName,
                updateRecurringReceiptEvent.RecurringReceipt.DateInitialRecurrence,
                updateRecurringReceiptEvent.RecurringReceipt.DateEndRecurrence,
                updateRecurringReceiptEvent.RecurringReceipt.RecurrenceTotalPrice,
                updateRecurringReceiptEvent.RecurringReceipt.Observation);
        }

        public static SpendManagementEvent ToEventDomain(this CreateRecurringReceiptEvent createRecurringReceiptEvent)
        {
            return new SpendManagementEvent(
                createRecurringReceiptEvent.RoutingKey,
                createRecurringReceiptEvent.EventCreatedDate,
                nameof(CreateRecurringReceiptEvent),
                JsonConvert.SerializeObject(createRecurringReceiptEvent));
        }

        public static SpendManagementEvent ToEventDomain(this UpdateRecurringReceiptEvent updateRecurringReceiptEvent)
        {
            return new SpendManagementEvent(
                updateRecurringReceiptEvent.RoutingKey,
                updateRecurringReceiptEvent.EventCreatedDate,
                nameof(UpdateRecurringReceiptEvent),
                JsonConvert.SerializeObject(updateRecurringReceiptEvent));
        }

        public static SpendManagementEvent ToEventDomain(this DeleteRecurringReceiptEvent deleteRecurringReceiptEvent)
        {
            return new SpendManagementEvent(
                deleteRecurringReceiptEvent.RoutingKey,
                deleteRecurringReceiptEvent.EventCreatedDate,
                nameof(DeleteRecurringReceiptEvent),
                JsonConvert.SerializeObject(deleteRecurringReceiptEvent));
        }

        public static CreateRecurringReceiptEvent ToRecurringReceiptCreatedEvent(this CreateRecurringReceiptCommand createRecurringReceiptCommand)
        {
            var recurringReceipt = new SpendManagement.Contracts.Contracts.V1.Entities.RecurringReceipt(createRecurringReceiptCommand.RecurringReceipt.Id,
                createRecurringReceiptCommand.RecurringReceipt.CategoryId,
                createRecurringReceiptCommand.RecurringReceipt.EstablishmentName,
                createRecurringReceiptCommand.RecurringReceipt.DateInitialRecurrence,
                createRecurringReceiptCommand.RecurringReceipt.DateEndRecurrence,
                createRecurringReceiptCommand.RecurringReceipt.RecurrenceTotalPrice,
                createRecurringReceiptCommand.RecurringReceipt.Observation);

            return new CreateRecurringReceiptEvent(recurringReceipt);
        }

        public static UpdateRecurringReceiptEvent ToUpdateRecurringReceiptEvent(this UpdateRecurringReceiptCommand updateRecurringReceiptCommand)
        {
            var recurringReceipt = new SpendManagement.Contracts.Contracts.V1.Entities.RecurringReceipt(updateRecurringReceiptCommand.RecurringReceipt.Id,
                updateRecurringReceiptCommand.RecurringReceipt.CategoryId,
                updateRecurringReceiptCommand.RecurringReceipt.EstablishmentName,
                updateRecurringReceiptCommand.RecurringReceipt.DateInitialRecurrence,
                updateRecurringReceiptCommand.RecurringReceipt.DateEndRecurrence,
                updateRecurringReceiptCommand.RecurringReceipt.RecurrenceTotalPrice,
                updateRecurringReceiptCommand.RecurringReceipt.Observation);

            return new UpdateRecurringReceiptEvent(recurringReceipt);
        }


        public static DeleteRecurringReceiptEvent ToDeleteRecurringReceiptEvent(this DeleteRecurringReceiptCommand deleteReceiptCommand)
        {
            return new DeleteRecurringReceiptEvent(deleteReceiptCommand.Id);
        }
    }
}
