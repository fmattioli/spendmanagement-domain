using Domain.Entities;
using SpendManagement.Contracts.V1.Entities;

namespace Application.Kafka.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDomain ToDomain(this Category category)
        {
            return new CategoryDomain(category.Id, category.Name, category.CreatedDate);
        }
    }
}
