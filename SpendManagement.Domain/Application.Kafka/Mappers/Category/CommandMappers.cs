using Domain.Entities;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;

namespace Application.Kafka.Mappers.Category
{
    public static class CommandMappers
    {
        public static Command ToDomain(this CreateCategoryCommand createCategoryCommand)
        {
            return new Command(createCategoryCommand.RoutingKey,
                createCategoryCommand.CommandCreatedDate,
                nameof(CreateCategoryCommand),
                JsonConvert.SerializeObject(createCategoryCommand));
        }

        public static Command ToDomain(this UpdateCategoryCommand updateCategoryCommand)
        {
            return new Command(updateCategoryCommand.RoutingKey,
                updateCategoryCommand.CommandCreatedDate,
                nameof(UpdateCategoryCommand),
                JsonConvert.SerializeObject(updateCategoryCommand));
        }

        public static Command ToDomain(this DeleteCategoryCommand deleteCategoryCommand)
        {
            return new Command(deleteCategoryCommand.RoutingKey,
                deleteCategoryCommand.CommandCreatedDate,
                nameof(DeleteCategoryCommand),
                JsonConvert.SerializeObject(deleteCategoryCommand));
        }
    }
}
