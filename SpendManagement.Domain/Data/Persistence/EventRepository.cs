using Data.Session;
using Domain.Entities;
using Domain.Interfaces;

namespace Data.Persistence
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        private readonly DbSession _dbSession;
        public EventRepository(DbSession dbSession) : base(dbSession)
        {
            _dbSession = dbSession;
        }
    }
}
