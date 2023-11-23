using Data.Session;
using Domain.Entities;
using Domain.Interfaces;
using Serilog;

namespace Data.Persistence
{
    public class SpendManagementEventRepository : BaseRepository<SpendManagementEvent>, ISpendManagementEventRepository
    {
        public SpendManagementEventRepository(DbSession dbSession, ILogger logger) : base(dbSession, logger)
        {
        }
    }
}
