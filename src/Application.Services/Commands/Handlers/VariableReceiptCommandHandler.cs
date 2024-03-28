using Application.Services.Mappers.Receipt;
using Application.Services.Events.Interfaces;
using KafkaFlow;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using Data.Persistence.Interfaces;
using Domain.Interfaces;
using MongoDB.Driver;
using Domain.Entities;
using MongoDB.Driver.Linq;

namespace Application.Services.Commands.Handlers
{
    public class VariableReceiptCommandHandler(IEventProducer receiptProducer, IUnitOfWork unitOfWork, IVariableReceiptRepository variableReceiptRepository) :
        IMessageHandler<CreateReceiptCommand>,
        IMessageHandler<UpdateReceiptCommand>,
        IMessageHandler<DeleteReceiptCommand>
    {
        private readonly IEventProducer _eventProducer = receiptProducer;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IVariableReceiptRepository _variableReceiptRepository = variableReceiptRepository;

        public async Task Handle(IMessageContext context, CreateReceiptCommand message)
        {
            var commandDomain = message.ToCommandDomain();
            await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var receiptEntity = message.ToDomainEntity();
            await _variableReceiptRepository.AddOneAsync(receiptEntity);

            var receiptCreatedEvent = message.ToReceiptCreatedEvent();
            await _eventProducer.SendEventAsync(receiptCreatedEvent);

            await _unitOfWork.SpendManagementEventRepository.Add(receiptCreatedEvent.ToEventDomain());
            _unitOfWork.Commit();
        }

        public async Task Handle(IMessageContext context, UpdateReceiptCommand message)
        {
            var commandDomain = message.ToCommandDomain();
            await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var receiptEntity = message.ToDomainEntity();
            var filter = new FilterDefinitionBuilder<ReceiptEntity>()
                .Where(m => m.Id == receiptEntity.Id);
            await _variableReceiptRepository.ReplaceOneAsync(_ => filter.Inject(), receiptEntity);

            var receiptUpdatedEvent = message.ToUpdateReceiptEvent();
            await _eventProducer.SendEventAsync(receiptUpdatedEvent);

            await _unitOfWork.SpendManagementEventRepository.Add(receiptUpdatedEvent.ToEventDomain());
            _unitOfWork.Commit();
        }

        public async Task Handle(IMessageContext context, DeleteReceiptCommand message)
        {
            var commandDomain = message.ToCommandDomain();
            await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var filter = new FilterDefinitionBuilder<ReceiptEntity>()
                .Where(ev => ev.Id == message.Id);
            await _variableReceiptRepository.DeleteAsync(_ => filter.Inject());

            var deleteReceiptEvent = message.ToDeleteReceiptEvent();
            await _eventProducer.SendEventAsync(deleteReceiptEvent);

            await _unitOfWork.SpendManagementEventRepository.Add(deleteReceiptEvent.ToEventDomain());
            _unitOfWork.Commit();
        }
    }
}
