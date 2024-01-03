using Domain.Entities;
using Domain.Interfaces;
using Serilog;
using System.Data.SqlClient;
using System.Data;

namespace Data.Persistence.Repository
{
    public class SpendManagementEventRepository(SqlConnection sqlConnection, ILogger logger, IDbTransaction dbTransaction) 
        : BaseRepository<SpendManagementEvent>(sqlConnection, dbTransaction, logger), ISpendManagementEventRepository
    {
    }
}
