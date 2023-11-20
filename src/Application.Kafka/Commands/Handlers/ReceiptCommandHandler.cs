using Application.Kafka.Mappers.Receipt;
using Application.Kafka.Events.Interfaces;
using Data.Statements;
using Domain.Interfaces;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;

namespace Application.Kafka.Commands.Handlers
{
    public class ReceiptCommandHandler :
        IMessageHandler<CreateReceiptCommand>,
        IMessageHandler<UpdateReceiptCommand>,
        IMessageHandler<DeleteReceiptCommand>
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventProducer _eventProducer;

        public ReceiptCommandHandler(ICommandRepository commandRepository,
            IEventRepository eventRepository,
            IEventProducer receiptProducer)
        {
            this._commandRepository = commandRepository;
            this._eventRepository = eventRepository;
            this._eventProducer = receiptProducer;
        }

        public async Task Handle(IMessageContext context, CreateReceiptCommand message)
        {
            var commandDomain = message.ToDomain();

            var commandId = await _commandRepository.Add(commandDomain);

            var receiptCreatedEvent = message.ToReceiptCreatedEvent();

            await _eventProducer.SendEventAsync(receiptCreatedEvent);

            await _eventRepository.Add(receiptCreatedEvent.ToDomain(commandId));
        }

        public async Task Handle(IMessageContext context, UpdateReceiptCommand message)
        {
            var commandDomain = message.ToDomain();

            var commandId = await _commandRepository.Add(commandDomain);

            var receiptUpdatedEvent = message.ToUpdateReceiptEvent();

            await _eventProducer.SendEventAsync(receiptUpdatedEvent);

            await _eventRepository.Add(receiptUpdatedEvent.ToDomain(commandId));
        }

        public async Task Handle(IMessageContext context, DeleteReceiptCommand message)
        {
            var commandDomain = message.ToDomain();

            var commandId = await _commandRepository.Add(commandDomain);

            var deleteReceiptEvent = message.ToDeleteReceiptEvent();

            await _eventProducer.SendEventAsync(deleteReceiptEvent);

            await _eventRepository.Add(deleteReceiptEvent.ToDomain(commandId));
        }
    }
}
