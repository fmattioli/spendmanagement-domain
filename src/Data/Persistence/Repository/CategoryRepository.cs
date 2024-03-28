using Domain.Entities;
using Domain.Interfaces;
using MongoDB.Driver;
using Serilog;

namespace Data.Persistence.Repository
{
    public class CategoryRepository(IMongoDatabase mongoDb, ILogger logger) : BaseRepository<CategoryEntity>(mongoDb, "Categories", logger), ICategoryRepository
    {
    }
}
