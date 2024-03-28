using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class CategoryEntity(Guid id, string name, DateTime createdDate)
    {
        [BsonId]
        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;
        public DateTime CreatedDate { get; set; } = createdDate;
    }
}
