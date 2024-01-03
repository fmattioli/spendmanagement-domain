using Domain.Entities;
using Domain.Interfaces;
using Serilog;
using System.Data;
using System.Data.SqlClient;

namespace Data.Persistence.Repository
{
    public class SpendManagementCommandRepository(SqlConnection sqlConnection, ILogger logger, IDbTransaction dbTransaction) 
        : BaseRepository<SpendManagementCommand>(sqlConnection, dbTransaction, logger), ISpendManagementCommandRepository
    {
    }
}
