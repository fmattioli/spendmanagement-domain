using Application.Kafka.Mappers.Category ;
using Application.Kafka.Events.Interfaces;
using Domain.Interfaces;
using KafkaFlow;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using Data.Persistence.Interfaces;
using Data.Persistence.UnitOfWork;

namespace Application.Kafka.Commands.Handlers
{
    public class CategoryCommandHandler
        : IMessageHandler<CreateCategoryCommand>,
        IMessageHandler<UpdateCategoryCommand>,
        IMessageHandler<DeleteCategoryCommand>

    {
        private readonly IEventProducer _eventProducer;
        private readonly ISpendManagementCommandRepository _commandRepository;
        private readonly ISpendManagementEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryCommandHandler(
            ISpendManagementCommandRepository commandRepository,
            ISpendManagementEventRepository eventRepository,
            IEventProducer eventProducer,
            IUnitOfWork unitOfWork)
            => (_commandRepository, _eventRepository, _eventProducer, _unitOfWork) = (commandRepository, eventRepository, eventProducer, unitOfWork);

        public async Task Handle(IMessageContext context, CreateCategoryCommand message)
        {
            var commandDomain = message.ToDomain();

            var commandId = await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var createCategoryEvent = message.ToCreateCategoryEvent();

            await _eventProducer.SendEventAsync(createCategoryEvent);

            var eventDomain = createCategoryEvent.ToDomain(commandId);
            await _unitOfWork.SpendManagementEventRepository.Add(eventDomain);

            _unitOfWork.Commit();
        }

        public async Task Handle(IMessageContext context, UpdateCategoryCommand message)
        {
            var commandDomain = message.ToDomain();

            var commandId = await _commandRepository.Add(commandDomain);

            var updateCategoryEvent = message.ToUpdateCategoryEvent();

            await _eventProducer.SendEventAsync(updateCategoryEvent);

            await _eventRepository.Add(updateCategoryEvent.ToDomain(commandId));
        }

        public async Task Handle(IMessageContext context, DeleteCategoryCommand message)
        {
            var commandDomain = message.ToDomain();

            var commandId = await _commandRepository.Add(commandDomain);

            var deleteCategoryEvent = message.ToDeleteCategoryEvent();

            await _eventProducer.SendEventAsync(deleteCategoryEvent);

            await _eventRepository.Add(deleteCategoryEvent.ToDomain(commandId));
        }
    }
}