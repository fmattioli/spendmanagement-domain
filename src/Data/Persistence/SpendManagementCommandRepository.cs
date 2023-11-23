using Data.Session;
using Domain.Entities;
using Domain.Interfaces;
using Serilog;

namespace Data.Persistence
{
    public class SpendManagementCommandRepository : BaseRepository<SpendManagementCommand>, ISpendManagementCommandRepository
    {
        public SpendManagementCommandRepository(DbSession dbSession, ILogger logger) : base(dbSession, logger)
        {
        }
    }
}
