using Application.Kafka.Converters;
using Application.Kafka.Events.Interfaces;
using Application.Kafka.Mappers;
using Data.Statements;
using Domain.Interfaces;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Serilog;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Contracts.V1.Events.ReceiptEvents;

namespace Application.Kafka.Commands.Handlers
{
    public class ReceiptCommandHandler : IMessageHandler<CreateReceiptCommand>
    {
        private readonly ILogger log;
        private readonly ICommandRepository _commandRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventProducer _eventProducer;

        public ReceiptCommandHandler(ILogger log, 
            ICommandRepository commandRepository,
            IEventRepository eventRepository,
            IEventProducer receiptProducer)
        {
            this.log = log;
            this._commandRepository = commandRepository;
            this._eventRepository = eventRepository;
            this._eventProducer = receiptProducer;
        }

        public async Task Handle(IMessageContext context, CreateReceiptCommand message)
        {
            var commandDomain = message.ToDomain();
            var receiptId = await _commandRepository.Add(commandDomain, SQLStatements.InsertCommand());

            var receiptCreatedEvent = message.ToReceiptCreatedEvent();
            await _eventProducer.SendEventAsync(receiptCreatedEvent);

            await _eventRepository.Add(receiptCreatedEvent.ToDomain(), SQLStatements.InsertEvent());
            log.Information(
                $"Command {nameof(CreateReceiptCommand)} successfully converted in a event {nameof(CreatedReceiptEvent)}and saved on database.",
                () => new
                {
                    commandDomain
                });
        }
    }
}
