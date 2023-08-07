using Data.Session;
using Domain.Entities;
using Domain.Interfaces;

namespace Data.Persistence
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(DbSession dbSession) : base(dbSession)
        {
        }
    }
}
