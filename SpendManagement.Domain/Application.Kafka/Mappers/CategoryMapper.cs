using Domain.Entities;
using SpendManagement.Contracts.V1.Entities;
using SpendManagement.Contracts.V1.Events.CategoryEvents;

namespace Application.Kafka.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDomain ToDomain(this Category category)
        {
            return new CategoryDomain(category.Id, category.Name, category.CreatedDate);
        }

        public static CreateCategoryEvents ToEvent(this CategoryDomain categoryDomain)
        {
            return new CreateCategoryEvents
            {
                Category = new Category(categoryDomain.Id, categoryDomain.Name),
                EventCreatedDate = categoryDomain.CreatedDate,
            };
        }
    }
}
