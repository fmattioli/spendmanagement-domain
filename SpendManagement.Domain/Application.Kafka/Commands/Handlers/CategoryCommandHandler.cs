using Application.Kafka.Mappers.Category ;
using Application.Kafka.Events.Interfaces;
using Data.Statements;
using Domain.Interfaces;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Serilog;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Events.CategoryEvents;

namespace Application.Kafka.Commands.Handlers
{
    public class CategoryCommandHandler
        : IMessageHandler<CreateCategoryCommand>,
        IMessageHandler<UpdateCategoryCommand>,
        IMessageHandler<DeleteCategoryCommand>
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

            var commandId = await _commandRepository.Add(commandDomain, SQLStatements.InsertCommand());

            var createCategoryEvent = message.ToCreateCategoryEvent();

            await _eventProducer.SendEventAsync(createCategoryEvent);

            await _eventRepository.Add(createCategoryEvent.ToDomain(commandId), SQLStatements.InsertEvent());

            _log.Information(
                "Command {nameof(CreateCategoryCommand)} successfully converted in a event {nameof(CreateCategoryEvent)}and saved on database.",
                () => new
                {
                    commandDomain
                });
        }

        public async Task Handle(IMessageContext context, UpdateCategoryCommand message)
        {
            var commandDomain = message.ToDomain();

            var commandId = await _commandRepository.Add(commandDomain, SQLStatements.InsertCommand());

            var updateCategoryEvent = message.ToUpdateCategoryEvent();
            await _eventProducer.SendEventAsync(updateCategoryEvent);

            await _eventRepository.Add(updateCategoryEvent.ToDomain(commandId), SQLStatements.InsertEvent());

            _log.Information(
                "Command {nameof(UpdateCategoryCommand)} successfully converted in a event {nameof(CreateCategoryEvent)}and saved on database.",
                () => new
                {
                    commandDomain
                });
        }

        public async Task Handle(IMessageContext context, DeleteCategoryCommand message)
        {
            var commandDomain = message.ToDomain();
            var commandId = await _commandRepository.Add(commandDomain, SQLStatements.InsertCommand());

            var deleteCategoryEvent = message.ToDeleteCategoryEvent();
            await _eventProducer.SendEventAsync(deleteCategoryEvent);

            await _eventRepository.Add(deleteCategoryEvent.ToDomain(commandId), SQLStatements.InsertEvent());

            _log.Information(
                "Command {nameof(DeleteCategoryCommand)} successfully converted in a event {nameof(CreateCategoryEvent)}and saved on database.",
                () => new
                {
                    commandDomain
                });
        }
    }
}