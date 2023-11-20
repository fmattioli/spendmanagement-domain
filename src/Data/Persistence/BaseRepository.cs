using Data.Session;
using Domain.Interfaces;
using Dapper;
using Serilog;
using Data.Statements;

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
            _db.Connection = _db.OpenConnection();

            using var conn = _db.Connection;
            var Id = await conn.ExecuteScalarAsync<int>(SQLStatements.InsertCommand(), entity);

            _logger.Information("Command or event inserted with sucessfully on database {@entity}", entity);

            return Id;
        }
    }
}
