using Domain.Entities;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.RecurringReceiptCommands;
using SpendManagement.Contracts.V1.Events.RecurringReceiptEvents;

namespace Application.Kafka.Mappers.RecurringReceipt
{
    public static class RecurringReceiptMappers
    {
        public static SpendManagementCommand ToDomain(this CreateRecurringReceiptCommand createReceiptCommand)
        {
            return new SpendManagementCommand(createReceiptCommand.RoutingKey,
                createReceiptCommand.CommandCreatedDate,
                nameof(CreateRecurringReceiptCommand),
                JsonConvert.SerializeObject(createReceiptCommand));
        }

        public static SpendManagementCommand ToDomain(this UpdateRecurringReceiptCommand updateReceiptCommand)
        {
            return new SpendManagementCommand(updateReceiptCommand.RoutingKey,
                updateReceiptCommand.CommandCreatedDate,
                nameof(UpdateRecurringReceiptCommand),
                JsonConvert.SerializeObject(updateReceiptCommand));
        }

        public static SpendManagementCommand ToDomain(this DeleteRecurringReceiptCommand deleteReceiptCommand)
        {
            return new SpendManagementCommand(deleteReceiptCommand.RoutingKey,
                deleteReceiptCommand.CommandCreatedDate,
                nameof(DeleteRecurringReceiptCommand),
                JsonConvert.SerializeObject(deleteReceiptCommand));
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
