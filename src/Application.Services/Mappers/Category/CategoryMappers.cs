using Domain.Entities;
using Newtonsoft.Json;
using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Events.CategoryEvents;

namespace Application.Services.Mappers.Category
{
    public static class CategoryMappers
    {
        public static CategoryEntity ToDomainEntity(this SpendManagement.Contracts.V1.Entities.Category command)
        {
            return new CategoryEntity(command.Id, command.Name, command.CreatedDate);
        }

        public static SpendManagementCommand ToCommandDomain(this CreateCategoryCommand createCategoryCommand)
        {
            return new SpendManagementCommand(createCategoryCommand.RoutingKey,
                createCategoryCommand.CommandCreatedDate,
                nameof(CreateCategoryCommand),
                JsonConvert.SerializeObject(createCategoryCommand));
        }

        public static SpendManagementCommand ToCommandDomain(this UpdateCategoryCommand updateCategoryCommand)
        {
            return new SpendManagementCommand(updateCategoryCommand.RoutingKey,
                updateCategoryCommand.CommandCreatedDate,
                nameof(UpdateCategoryCommand),
                JsonConvert.SerializeObject(updateCategoryCommand));
        }

        public static SpendManagementCommand ToCommandDomain(this DeleteCategoryCommand deleteCategoryCommand)
        {
            return new SpendManagementCommand(deleteCategoryCommand.RoutingKey,
                deleteCategoryCommand.CommandCreatedDate,
                nameof(DeleteCategoryCommand),
                JsonConvert.SerializeObject(deleteCategoryCommand));
        }

        public static SpendManagementEvent ToEventDomain(this CreateCategoryEvent createCategoryEvent)
        {
            return new SpendManagementEvent(
                createCategoryEvent.RoutingKey,
                createCategoryEvent.EventCreatedDate,
                nameof(CreateCategoryEvent),
                JsonConvert.SerializeObject(createCategoryEvent));
        }

        public static SpendManagementEvent ToEventDomain(this UpdateCategoryEvent createCategoryEvent)
        {
            return new SpendManagementEvent(
                createCategoryEvent.RoutingKey,
                createCategoryEvent.EventCreatedDate,
                nameof(CreateCategoryEvent),
                JsonConvert.SerializeObject(createCategoryEvent));
        }

        public static SpendManagementEvent ToEventDomain(this DeleteCategoryEvent deleteCategoryEvent)
        {
            return new SpendManagementEvent(
                deleteCategoryEvent.RoutingKey,
                deleteCategoryEvent.EventCreatedDate,
                nameof(DeleteCategoryEvent),
                JsonConvert.SerializeObject(deleteCategoryEvent));
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

        public static DeleteCategoryEvent ToDeleteCategoryEvent(this DeleteCategoryCommand deleteCategoryCommand)
        {
            return new DeleteCategoryEvent(deleteCategoryCommand.Id);
        }
    }
}
