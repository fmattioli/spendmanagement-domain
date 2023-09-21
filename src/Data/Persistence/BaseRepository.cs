using Data.Session;
using Domain.Interfaces;
using Dapper;

namespace Data.Persistence
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DbSession _db;

        public BaseRepository(DbSession dbSession) => this._db = dbSession;

        public async Task<int> Add(T entity, string sqlCommand)
        {
            _db.Connection = _db.OpenConnection();
            using var conn = _db.Connection;
            return await conn.ExecuteScalarAsync<int>(sqlCommand, entity);
        }
    }
}
