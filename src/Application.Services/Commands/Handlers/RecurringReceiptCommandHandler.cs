using Application.Services.Events.Interfaces;
using Application.Services.Mappers.RecurringReceipt;
using Data.Persistence.Interfaces;
using Data.Persistence.Repository;
using Domain.Entities;
using Domain.Interfaces;
using KafkaFlow;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SpendManagement.Contracts.V1.Commands.RecurringReceiptCommands;

namespace Application.Services.Commands.Handlers
{
    public class RecurringReceiptCommandHandler(IEventProducer recurringReceiptProducer, IUnitOfWork unitOfWork, IRecurringReceiptRepository recurringReceiptRepository) :
        IMessageHandler<CreateRecurringReceiptCommand>,
        IMessageHandler<UpdateRecurringReceiptCommand>,
        IMessageHandler<DeleteRecurringReceiptCommand>
    {
        private readonly IEventProducer _eventProducer = recurringReceiptProducer;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IRecurringReceiptRepository _recurringReceiptRepository = recurringReceiptRepository;

        public async Task Handle(IMessageContext context, CreateRecurringReceiptCommand message)
        {
            var commandDomain = message.ToCommandDomain();
            await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var receiptEntity = message.ToDomainEntity();
            await _recurringReceiptRepository.AddOneAsync(receiptEntity);

            var receiptCreatedEvent = message.ToRecurringReceiptCreatedEvent();
            await _eventProducer.SendEventAsync(receiptCreatedEvent);

            await _unitOfWork.SpendManagementEventRepository.Add(receiptCreatedEvent.ToEventDomain());
            _unitOfWork.Commit();
        }

        public async Task Handle(IMessageContext context, UpdateRecurringReceiptCommand message)
        {
            var commandDomain = message.ToCommandDomain();
            await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var receiptEntity = message.ToDomainEntity();
            var filter = new FilterDefinitionBuilder<RecurringReceiptEntity>()
                .Where(m => m.Id == receiptEntity.Id);
            await _recurringReceiptRepository.ReplaceOneAsync(_ => filter.Inject(), receiptEntity);

            var receiptUpdatedEvent = message.ToUpdateRecurringReceiptEvent();
            await _eventProducer.SendEventAsync(receiptUpdatedEvent);

            await _unitOfWork.SpendManagementEventRepository.Add(receiptUpdatedEvent.ToEventDomain());
            _unitOfWork.Commit();
        }

        public async Task Handle(IMessageContext context, DeleteRecurringReceiptCommand message)
        {
            var commandDomain = message.ToCommandDomain();
            await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var filter = new FilterDefinitionBuilder<RecurringReceiptEntity>()
              .Where(ev => ev.Id == message.Id);
            await _recurringReceiptRepository.DeleteAsync(_ => filter.Inject());

            var deleteReceiptEvent = message.ToDeleteRecurringReceiptEvent();
            await _eventProducer.SendEventAsync(deleteReceiptEvent);

            await _unitOfWork.SpendManagementEventRepository.Add(deleteReceiptEvent.ToEventDomain());
            _unitOfWork.Commit();
        }
    }
}
