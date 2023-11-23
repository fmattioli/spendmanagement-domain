using Data.Session;
using Domain.Interfaces;
using Dapper;
using Serilog;
using Data.Statements;
using Domain.Entities;

namespace Data.Persistence
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DbSession _db;
        private readonly ILogger _logger;

        public BaseRepository(DbSession dbSession, ILogger logger)
        {
            this._db = dbSession;
            this._logger = logger;
        }

        public async Task<int> Add(T entity)
        {
            using var conn = _db.OpenConnection();

            string sqlStatement = entity switch
            {
                SpendManagementCommand _ => SQLStatements.InsertCommand(),
                _ => SQLStatements.InsertEvent()
            };

            var id = await conn.ExecuteScalarAsync<int>(sqlStatement, entity);

            _logger.Information("Command or event inserted successfully on database {@entity}", entity);

            return id;
        }
    }
}
