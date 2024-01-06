using Application.Kafka.Events.Interfaces;
using Application.Kafka.Mappers.Category;
using Data.Persistence.Interfaces;
using KafkaFlow;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;

namespace Application.Kafka.Commands.Handlers
{
    public class CategoryCommandHandler
        : IMessageHandler<CreateCategoryCommand>,
        IMessageHandler<UpdateCategoryCommand>,
        IMessageHandler<DeleteCategoryCommand>

    {
        private readonly IEventProducer _eventProducer;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryCommandHandler(
            IEventProducer eventProducer,
            IUnitOfWork unitOfWork)
            => (_eventProducer, _unitOfWork) = (eventProducer, unitOfWork);

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

            var commandId = await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var updateCategoryEvent = message.ToUpdateCategoryEvent();

            await _eventProducer.SendEventAsync(updateCategoryEvent);

            var eventDomain = updateCategoryEvent.ToDomain(commandId);
            await _unitOfWork.SpendManagementEventRepository.Add(eventDomain);

            _unitOfWork.Commit();
        }

        public async Task Handle(IMessageContext context, DeleteCategoryCommand message)
        {
            var commandDomain = message.ToDomain();

            var commandId = await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var deleteCategoryEvent = message.ToDeleteCategoryEvent();

            await _eventProducer.SendEventAsync(deleteCategoryEvent);

            var eventDomain = deleteCategoryEvent.ToDomain(commandId);
            await _unitOfWork.SpendManagementEventRepository.Add(eventDomain);

            _unitOfWork.Commit();
        }
    }
}