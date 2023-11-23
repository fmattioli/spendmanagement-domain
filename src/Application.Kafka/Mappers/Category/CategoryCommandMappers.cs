using Domain.Entities;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;

namespace Application.Kafka.Mappers.Category
{
    public static class CategoryCommandMappers
    {
        public static SpendManagementCommand ToDomain(this CreateCategoryCommand createCategoryCommand)
        {
            return new SpendManagementCommand(createCategoryCommand.RoutingKey,
                createCategoryCommand.CommandCreatedDate,
                nameof(CreateCategoryCommand),
                JsonConvert.SerializeObject(createCategoryCommand));
        }

        public static SpendManagementCommand ToDomain(this UpdateCategoryCommand updateCategoryCommand)
        {
            return new SpendManagementCommand(updateCategoryCommand.RoutingKey,
                updateCategoryCommand.CommandCreatedDate,
                nameof(UpdateCategoryCommand),
                JsonConvert.SerializeObject(updateCategoryCommand));
        }

        public static SpendManagementCommand ToDomain(this DeleteCategoryCommand deleteCategoryCommand)
        {
            return new SpendManagementCommand(deleteCategoryCommand.RoutingKey,
                deleteCategoryCommand.CommandCreatedDate,
                nameof(DeleteCategoryCommand),
                JsonConvert.SerializeObject(deleteCategoryCommand));
        }
    }
}
