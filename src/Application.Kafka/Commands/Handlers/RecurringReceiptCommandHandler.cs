using Application.Kafka.Events.Interfaces;
using Application.Kafka.Mappers.RecurringReceipt;
using Data.Persistence.Interfaces;
using KafkaFlow;
using SpendManagement.Contracts.V1.Commands.RecurringReceiptCommands;

namespace Application.Kafka.Commands.Handlers
{
    public class RecurringReceiptCommandHandler(IEventProducer recurringReceiptProducer, IUnitOfWork unitOfWork) :
        IMessageHandler<CreateRecurringReceiptCommand>,
        IMessageHandler<UpdateRecurringReceiptCommand>,
        IMessageHandler<DeleteRecurringReceiptCommand>
    {
        private readonly IEventProducer _eventProducer = recurringReceiptProducer;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(IMessageContext context, CreateRecurringReceiptCommand message)
        {
            var commandDomain = message.ToDomain();

            await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var receiptCreatedEvent = message.ToRecurringReceiptCreatedEvent();

            _unitOfWork.Commit();

            await _eventProducer.SendEventAsync(receiptCreatedEvent);
        }

        public async Task Handle(IMessageContext context, UpdateRecurringReceiptCommand message)
        {
            var commandDomain = message.ToDomain();

            await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var receiptUpdatedEvent = message.ToUpdateRecurringReceiptEvent();

            await _eventProducer.SendEventAsync(receiptUpdatedEvent);

            _unitOfWork.Commit();
        }

        public async Task Handle(IMessageContext context, DeleteRecurringReceiptCommand message)
        {
            var commandDomain = message.ToDomain();

            await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var deleteReceiptEvent = message.ToDeleteRecurringReceiptEvent();

            await _eventProducer.SendEventAsync(deleteReceiptEvent);

            _unitOfWork.Commit();
        }
    }
}
