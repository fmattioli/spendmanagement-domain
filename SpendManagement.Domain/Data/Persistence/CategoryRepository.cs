using Data.Session;
using Domain.Entities;
using Domain.Interfaces;

namespace Data.Persistence
{
    public class CategoryRepository : BaseRepository<CategoryDomain>, ICategoryRepository
    {
        private readonly DbSession _dbSession;
        public CategoryRepository(DbSession dbSession) : base(dbSession)
        {
            _dbSession = dbSession;
        }
    }
}
