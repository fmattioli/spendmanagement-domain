using Domain.Entities;
using Domain.Interfaces;
using Npgsql;
using Serilog;
using System.Data;

namespace Data.Persistence.Repository
{
    public class SpendManagementCommandRepository(NpgsqlConnection connection, ILogger logger, IDbTransaction dbTransaction) 
        : BaseRepository<SpendManagementCommand>(connection, dbTransaction, logger), ISpendManagementCommandRepository
    {
    }
}
