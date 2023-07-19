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
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger _log;

        public CategoryCommandHandler(ICategoryRepository categoryRepository, ILogger log) => (_categoryRepository, _log) = (categoryRepository, log);

        public async Task Handle(IMessageContext context, CreateCategoryCommand message)
        {
            var category = message.Category.ToDomain();
            await _categoryRepository.Add(category, CategorySqlCommands.InsertCategory());
            _log.Information(
                $"Spent saved with successfully on database.",
                () => new
                {
                    category
                });
        }
    }
}
