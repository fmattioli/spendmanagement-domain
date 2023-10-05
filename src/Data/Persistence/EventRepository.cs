using Data.Session;
using Domain.Entities;
using Domain.Interfaces;
using Serilog;

namespace Data.Persistence
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(DbSession dbSession, ILogger logger) : base(dbSession, logger)
        {
        }
    }
}
