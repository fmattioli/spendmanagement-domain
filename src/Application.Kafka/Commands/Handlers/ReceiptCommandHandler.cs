using Application.Kafka.Mappers.Receipt;
using Application.Kafka.Events.Interfaces;
using KafkaFlow;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using Data.Persistence.Interfaces;

namespace Application.Kafka.Commands.Handlers
{
    public class ReceiptCommandHandler(IEventProducer receiptProducer, IUnitOfWork unitOfWork) :
        IMessageHandler<CreateReceiptCommand>,
        IMessageHandler<UpdateReceiptCommand>,
        IMessageHandler<DeleteReceiptCommand>
    {
        private readonly IEventProducer _eventProducer = receiptProducer;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(IMessageContext context, CreateReceiptCommand message)
        {
            var commandDomain = message.ToDomain();

            var commandId = await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var receiptCreatedEvent = message.ToReceiptCreatedEvent();

            await _eventProducer.SendEventAsync(receiptCreatedEvent);

            var eventDomain = receiptCreatedEvent.ToDomain(commandId);
            await _unitOfWork.SpendManagementEventRepository.Add(eventDomain);

            _unitOfWork.Commit();
        }

        public async Task Handle(IMessageContext context, UpdateReceiptCommand message)
        {
            var commandDomain = message.ToDomain();

            var commandId = await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var receiptUpdatedEvent = message.ToUpdateReceiptEvent();

            await _eventProducer.SendEventAsync(receiptUpdatedEvent);

            var eventDomain = receiptUpdatedEvent.ToDomain(commandId);
            await _unitOfWork.SpendManagementEventRepository.Add(eventDomain);

            _unitOfWork.Commit();
        }

        public async Task Handle(IMessageContext context, DeleteReceiptCommand message)
        {
            var commandDomain = message.ToDomain();

            var commandId = await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var deleteReceiptEvent = message.ToDeleteReceiptEvent();

            await _eventProducer.SendEventAsync(deleteReceiptEvent);

            var eventDomain = deleteReceiptEvent.ToDomain(commandId);
            await _unitOfWork.SpendManagementEventRepository.Add(eventDomain);

            _unitOfWork.Commit();
        }
    }
}
