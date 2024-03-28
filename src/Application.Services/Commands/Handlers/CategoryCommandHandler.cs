using Application.Services.Events.Interfaces;
using Application.Services.Mappers.Category;
using Data.Persistence.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using KafkaFlow;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;

namespace Application.Services.Commands.Handlers
{
    public class CategoryCommandHandler
        : IMessageHandler<CreateCategoryCommand>,
        IMessageHandler<UpdateCategoryCommand>,
        IMessageHandler<DeleteCategoryCommand>

    {
        private readonly IEventProducer _eventProducer;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryCommandHandler(
            IEventProducer eventProducer,
            IUnitOfWork unitOfWork,
            ICategoryRepository categoryRepository)
            => (_eventProducer, _unitOfWork, _categoryRepository) = (eventProducer, unitOfWork, categoryRepository);

        public async Task Handle(IMessageContext context, CreateCategoryCommand message)
        {
            var commandDomain = message.ToCommandDomain();
            await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var categoryDomain = message.Category.ToDomainEntity();
            await _categoryRepository.AddOneAsync(categoryDomain);

            var createCategoryEvent = message.ToCreateCategoryEvent();
            await _eventProducer.SendEventAsync(createCategoryEvent);

            await _unitOfWork.SpendManagementEventRepository.Add(createCategoryEvent.ToEventDomain());
            _unitOfWork.Commit();
        }

        public async Task Handle(IMessageContext context, UpdateCategoryCommand message)
        {
            var commandDomain = message.ToCommandDomain();
            await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var categoryDomain = message.Category.ToDomainEntity();
            var filter = new FilterDefinitionBuilder<CategoryEntity>()
               .Where(m => m.Id == categoryDomain.Id);

            await _categoryRepository.ReplaceOneAsync(_ => filter.Inject(), categoryDomain);

            var updateCategoryEvent = message.ToUpdateCategoryEvent();
            await _eventProducer.SendEventAsync(updateCategoryEvent);

            await _unitOfWork.SpendManagementEventRepository.Add(updateCategoryEvent.ToEventDomain());
            _unitOfWork.Commit();
        }

        public async Task Handle(IMessageContext context, DeleteCategoryCommand message)
        {
            var commandDomain = message.ToCommandDomain();
            await _unitOfWork.SpendManagementCommandRepository.Add(commandDomain);

            var filter = new FilterDefinitionBuilder<CategoryEntity>()
                .Where(ev => ev.Id == message.Id);
            await _categoryRepository.DeleteAsync(_ => filter.Inject());

            var deleteCategoryEvent = message.ToDeleteCategoryEvent();
            await _eventProducer.SendEventAsync(deleteCategoryEvent);

            await _unitOfWork.SpendManagementEventRepository.Add(deleteCategoryEvent.ToEventDomain());
            _unitOfWork.Commit();
        }
    }
}