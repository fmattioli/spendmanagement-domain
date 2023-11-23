using Domain.Entities;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Events.CategoryEvents;

namespace Application.Kafka.Mappers.Category
{
    public static class CategoryMappers
    {
        public static SpendManagementEvent ToDomain(this CreateCategoryEvent createCategoryEvent, int commandId)
        {
            return new SpendManagementEvent(commandId,
                createCategoryEvent.RoutingKey,
                createCategoryEvent.EventCreatedDate,
                nameof(CreateCategoryEvent),
                JsonConvert.SerializeObject(createCategoryEvent));
        }

        public static CreateCategoryEvent ToCreateCategoryEvent(this CreateCategoryCommand createCategoryCommand)
        {
            var category = new SpendManagement.Contracts.V1.Entities.Category(createCategoryCommand.Category.Id, createCategoryCommand.Category.Name);
            return new CreateCategoryEvent(category);
        }

        public static UpdateCategoryEvent ToUpdateCategoryEvent(this UpdateCategoryCommand updateCategoryCommand)
        {
            var category = new SpendManagement.Contracts.V1.Entities.Category(updateCategoryCommand.Category.Id, updateCategoryCommand.Category.Name);
            return new UpdateCategoryEvent(category);
        }

        public static SpendManagementEvent ToDomain(this UpdateCategoryEvent updateCategoryEvent, int commandId)
        {
            return new SpendManagementEvent(commandId,
                updateCategoryEvent.RoutingKey,
                updateCategoryEvent.EventCreatedDate,
                nameof(UpdateCategoryEvent),
                JsonConvert.SerializeObject(updateCategoryEvent));
        }

        public static DeleteCategoryEvent ToDeleteCategoryEvent(this DeleteCategoryCommand deleteCategoryCommand)
        {
            return new DeleteCategoryEvent(deleteCategoryCommand.Id);
        }

        public static SpendManagementEvent ToDomain(this DeleteCategoryEvent deleteCategoryEvent, int commandId)
        {
            return new SpendManagementEvent(commandId,
                deleteCategoryEvent.RoutingKey,
                deleteCategoryEvent.EventCreatedDate,
                nameof(DeleteCategoryEvent),
                JsonConvert.SerializeObject(deleteCategoryEvent));
        }
    }
}
