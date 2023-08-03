using Data.Session;
using Domain.Entities;
using Domain.Interfaces;

namespace Data.Persistence
{
    public class CommandRepository : BaseRepository<Command>, ICommandRepository
    {
        private readonly DbSession _dbSession;
        public CommandRepository(DbSession dbSession) : base(dbSession)
        {
            _dbSession = dbSession;
        }
    }
}
