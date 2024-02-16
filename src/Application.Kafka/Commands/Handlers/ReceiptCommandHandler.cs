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

            await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var receiptCreatedEvent = message.ToReceiptCreatedEvent();

            _unitOfWork.Commit();

            await _eventProducer.SendEventAsync(receiptCreatedEvent);
        }

        public async Task Handle(IMessageContext context, UpdateReceiptCommand message)
        {
            var commandDomain = message.ToDomain();

            await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var receiptUpdatedEvent = message.ToUpdateReceiptEvent();

            await _eventProducer.SendEventAsync(receiptUpdatedEvent);

            _unitOfWork.Commit();
        }

        public async Task Handle(IMessageContext context, DeleteReceiptCommand message)
        {
            var commandDomain = message.ToDomain();

            await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var deleteReceiptEvent = message.ToDeleteReceiptEvent();

            await _eventProducer.SendEventAsync(deleteReceiptEvent);

            _unitOfWork.Commit();
        }
    }
}
