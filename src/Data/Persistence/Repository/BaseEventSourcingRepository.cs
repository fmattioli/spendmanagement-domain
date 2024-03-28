using Domain.Interfaces;
using Dapper;
using Serilog;
using Data.Statements;
using System.Data;
using Npgsql;

namespace Data.Persistence.Repository
{
    public class BaseEventSourcingRepository<T>(NpgsqlConnection connection, IDbTransaction dbTransaction, ILogger logger) : IBaseEventSourcingRepository<T> where T : class
    {
        private readonly NpgsqlConnection _connection = connection;
        private readonly IDbTransaction _dbTransaction = dbTransaction;
        private readonly ILogger _logger = logger;

        public async Task<Guid> Add(T entity)
        {
            var sql = SQLStatements.InsertCommand();

            var result = await _connection.ExecuteScalarAsync(sql, entity, _dbTransaction);

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
