using Domain.Entities;
using Domain.Interfaces;
using Npgsql;
using Serilog;
using System.Data;

namespace Data.Persistence.Repository
{
    public class SpendManagementEventRepository(NpgsqlConnection connection, ILogger logger, IDbTransaction dbTransaction)
       : BaseEventSourcingRepository<SpendManagementEvent>(connection, dbTransaction, logger), ISpendManagementEventRepository
    {
    }
}
