using Domain.Interfaces;
using Dapper;
using Serilog;
using Data.Statements;
using Domain.Entities;
using System.Data;
using System.Data.SqlClient;

namespace Data.Persistence.Repository
{
    public class BaseRepository<T>(SqlConnection sqlConnection, IDbTransaction dbTransaction, ILogger logger) : IBaseRepository<T> where T : class
    {
        private readonly SqlConnection _sqlConnection = sqlConnection;
        private readonly IDbTransaction _dbTransaction = dbTransaction;
        private readonly ILogger _logger = logger;

        public async Task<int> Add(T entity)
        {
            string sqlStatement = entity switch
            {
                SpendManagementCommand _ => SQLStatements.InsertCommand(),
                _ => SQLStatements.InsertEvent()
            };

            var id = await _sqlConnection.ExecuteScalarAsync<int>(sqlStatement, entity, _dbTransaction);

            _logger.Information("Command or event inserted successfully on database {@entity}", entity);

            return id;
        }
    }
}
