using Domain.Interfaces;
using Dapper;
using Serilog;
using Data.Statements;
using System.Data;
using System.Data.SqlClient;

namespace Data.Persistence.Repository
{
    public class BaseRepository<T>(SqlConnection sqlConnection, IDbTransaction dbTransaction, ILogger logger) : IBaseRepository<T> where T : class
    {
        private readonly SqlConnection _sqlConnection = sqlConnection;
        private readonly IDbTransaction _dbTransaction = dbTransaction;
        private readonly ILogger _logger = logger;

        public async Task<Guid> Add(T entity)
        {
            var sql = SQLStatements.InsertCommand();

            var result = await _sqlConnection.ExecuteScalarAsync(sql, entity, _dbTransaction);

            if (result != null && Guid.TryParse(result.ToString(), out Guid guidResult))
            {
                _logger.Information("Command or event inserted successfully on database {@entity}", entity);
                return guidResult;
            }

            _logger.Information("An error occured when tried insert the command", entity);
            return Guid.Empty;
        }
    }
}
