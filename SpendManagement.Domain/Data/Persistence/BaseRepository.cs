using Data.Statements;
using Data.Session;
using Domain.Interfaces;
using Dapper;

namespace Data.Persistence
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbSession _db;

        public BaseRepository(DbSession dbSession) => this._db = dbSession;

        public async Task<int> Add(T entity, string table)
        {
            using var conn = _db.Connection;
            var Id = await conn.ExecuteScalarAsync<int>(
                SqlCommands.InsertReceipt(table), entity);
            return Id;
        }
    }
}
