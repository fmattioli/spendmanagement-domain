using Data.Session;
using Domain.Entities;
using Domain.Interfaces;

namespace Data.Persistence
{
    public class CommandRepository : BaseRepository<Command>, ICommandRepository
    {
        public CommandRepository(DbSession dbSession) : base(dbSession)
        {
        }
    }
}
