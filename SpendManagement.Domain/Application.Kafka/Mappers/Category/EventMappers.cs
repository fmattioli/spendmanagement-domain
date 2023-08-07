using Domain.Entities;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Events.CategoryEvents;

namespace Application.Kafka.Mappers.Category
{
    public static class EventMappers
    {
        public static Event ToDomain(this CreateCategoryEvent createCategoryEvent)
        {
            return new Event(createCategoryEvent.RoutingKey,
                createCategoryEvent.EventCreatedDate,
                nameof(CreateCategoryEvent),
                JsonConvert.SerializeObject(createCategoryEvent));
        }

        public static CreateCategoryEvent ToCreateCategoryEvent(this CreateCategoryCommand createCategoryCommand)
        {
            var category = new SpendManagement.Contracts.V1.Entities.Category(createCategoryCommand.Category.Id, createCategoryCommand.Category.Name);
            return new CreateCategoryEvent(createCategoryCommand.RoutingKey, category);
        }

        public static UpdateCategoryEvent ToUpdateCategoryEvent(this UpdateCategoryCommand updateCategoryCommand)
        {
            var category = new SpendManagement.Contracts.V1.Entities.Category(updateCategoryCommand.Category.Id, updateCategoryCommand.Category.Name);
            return new UpdateCategoryEvent(updateCategoryCommand.RoutingKey, category);
        }

        public static Event ToDomain(this UpdateCategoryEvent updateCategoryEvent)
        {
            return new Event(updateCategoryEvent.RoutingKey,
                updateCategoryEvent.EventCreatedDate,
                nameof(UpdateCategoryEvent),
                JsonConvert.SerializeObject(updateCategoryEvent));
        }

        public static DeleteCategoryEvent ToDeleteCategoryEvent(this DeleteCategoryCommand deleteCategoryCommand)
        {
            return new DeleteCategoryEvent(deleteCategoryCommand.RoutingKey, deleteCategoryCommand.Id);
        }

        public static Event ToDomain(this DeleteCategoryEvent deleteCategoryEvent)
        {
            return new Event(deleteCategoryEvent.RoutingKey,
                deleteCategoryEvent.EventCreatedDate,
                nameof(DeleteCategoryEvent),
                JsonConvert.SerializeObject(deleteCategoryEvent));
        }
    }
}
