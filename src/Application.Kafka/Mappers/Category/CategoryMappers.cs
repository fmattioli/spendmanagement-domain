using SpendManagement.Contracts.V1.Commands.CategoryCommands;
using SpendManagement.Contracts.V1.Events.CategoryEvents;

namespace Application.Kafka.Mappers.Category
{
    public static class CategoryMappers
    {
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
