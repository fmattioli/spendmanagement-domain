﻿using Application.Kafka.Mappers.Receipt;
using Application.Kafka.Events.Interfaces;
using Data.Statements;
using Domain.Interfaces;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Serilog;
using SpendManagement.Contracts.V1.Commands.ReceiptCommands;
using SpendManagement.Contracts.V1.Events.ReceiptEvents;

namespace Application.Kafka.Commands.Handlers
{
    public class ReceiptCommandHandler : 
        IMessageHandler<CreateReceiptCommand>,
        IMessageHandler<UpdateReceiptCommand>,
        IMessageHandler<DeleteReceiptCommand>
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

            var commandId = await _commandRepository.Add(commandDomain, SQLStatements.InsertCommand());

            var receiptCreatedEvent = message.ToReceiptCreatedEvent();
            await _eventProducer.SendEventAsync(receiptCreatedEvent);

            await _eventRepository.Add(receiptCreatedEvent.ToDomain(commandId), SQLStatements.InsertEvent());

            log.Information(
                $"Command {nameof(CreateReceiptCommand)} successfully converted in a event {nameof(CreatedReceiptEvent)}and saved on database.",
                () => new
                {
                    commandDomain
                });
        }

        public async Task Handle(IMessageContext context, UpdateReceiptCommand message)
        {
            var commandDomain = message.ToDomain();

            var commandId = await _commandRepository.Add(commandDomain, SQLStatements.InsertCommand());

            var receiptUpdatedEvent = message.ToUpdateReceiptEvent();
            await _eventProducer.SendEventAsync(receiptUpdatedEvent);

            await _eventRepository.Add(receiptUpdatedEvent.ToDomain(commandId), SQLStatements.InsertEvent());

            log.Information(
                $"Command {nameof(UpdateReceiptCommand)} successfully converted in a event {nameof(CreatedReceiptEvent)}and saved on database.",
                () => new
                {
                    commandDomain
                });
        }

        public async Task Handle(IMessageContext context, DeleteReceiptCommand message)
        {
            var commandDomain = message.ToDomain();

            var commandId = await _commandRepository.Add(commandDomain, SQLStatements.InsertCommand());

            var deleteReceiptEvent = message.ToDeleteReceiptEvent();
            await _eventProducer.SendEventAsync(deleteReceiptEvent);

            await _eventRepository.Add(deleteReceiptEvent.ToDomain(commandId), SQLStatements.InsertEvent());

            log.Information(
                $"Command {nameof(DeleteReceiptCommand)} successfully converted in a event {nameof(CreatedReceiptEvent)}and saved on database.",
                () => new
                {
                    commandDomain
                });
        }
    }
}