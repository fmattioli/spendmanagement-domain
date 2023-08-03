using Application.Kafka.Converters;
using Application.Kafka.Events.Interfaces;
using Application.Kafka.Mappers;
using Data.Statements;
using Domain.Interfaces;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Serilog;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Events.CategoryEvents;

namespace Application.Kafka.Commands.Handlers
{
    public class CategoryCommandHandler : IMessageHandler<CreateCategoryCommand>
    {
        private readonly IEventProducer _eventProducer;
        private readonly ICommandRepository _commandRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ILogger _log;

        public CategoryCommandHandler(ICommandRepository commandRepository, 
            IEventRepository eventRepository, 
            ILogger log, 
            IEventProducer eventProducer) 
            => (_commandRepository, _eventRepository, _log, _eventProducer) = (commandRepository, eventRepository, log, eventProducer);

        public async Task Handle(IMessageContext context, CreateCategoryCommand message)
        {
            var commandDomain = message.ToDomain();
            await _commandRepository.Add(commandDomain, SQLStatements.InsertCommand());

            var createCategoryEvent = message.ToCreateCategoryEvent();
            await _eventProducer.SendEventAsync(message.ToCreateCategoryEvent());
            await _eventRepository.Add(createCategoryEvent.ToDomain(), SQLStatements.InsertEvent());

            _log.Information(
                $"Command {nameof(CreateCategoryCommand)} successfully converted in a event {nameof(CreateCategoryEvent)}and saved on database.",
                () => new
                {
                    commandDomain
                });
        }
    }
}