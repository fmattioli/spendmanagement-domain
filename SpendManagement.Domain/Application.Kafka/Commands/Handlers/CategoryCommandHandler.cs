using Application.Kafka.Events.Interfaces;
using Application.Kafka.Mappers;
using Data.Statements;
using Domain.Interfaces;
using KafkaFlow;
using KafkaFlow.TypedHandler;
using Serilog;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;

namespace Application.Kafka.Commands.Handlers
{
    public class CategoryCommandHandler : IMessageHandler<CreateCategoryCommand>
    {
        private readonly IEventProducer _eventProducer;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger _log;

        public CategoryCommandHandler(ICategoryRepository categoryRepository, ILogger log, IEventProducer eventProducer) 
            => (_categoryRepository, _log, _eventProducer) = (categoryRepository, log, eventProducer);

        public async Task Handle(IMessageContext context, CreateCategoryCommand message)
        {
            var category = message.Category.ToDomain();
            await _categoryRepository.Add(category, CategorySqlCommands.InsertCategory());

            await _eventProducer.SendEventAsync(category.ToEvent());

            _log.Information(
                $"A new category was saved with successfully on database.",
                () => new
                {
                    category
                });
        }
    }
}