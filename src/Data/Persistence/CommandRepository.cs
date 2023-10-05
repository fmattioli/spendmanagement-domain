using Data.Session;
using Domain.Entities;
using Domain.Interfaces;
using Serilog;

namespace Data.Persistence
{
    public class CommandRepository : BaseRepository<Command>, ICommandRepository
    {
        public CommandRepository(DbSession dbSession, ILogger logger) : base(dbSession, logger)
        {
        }
    }
}
