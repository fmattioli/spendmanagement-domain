using Dapper;
using Data.Session;
using Data.Statements;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Data.Persistence
{
    public class ReceiptRepository : BaseRepository<ReceiptDomain>, IReceiptRepository
    {
        private readonly DbSession _dbSession;
        public ReceiptRepository(DbSession dbSession) : base(dbSession)
        {
            _dbSession = dbSession;
        }

        public async Task<bool> AddReceiptItem(Guid receiptId, IEnumerable<ReceiptItemDomain> receiptItems)
        {
            _db.Connection = _dbSession.OpenConnection();
            using var conn = _db.Connection;
            foreach (var receiptItem in receiptItems)
            {
                receiptItem.ReceiptId = receiptId;
                await conn.ExecuteScalarAsync(SqlCommands.InsertReceiptItems(), receiptItem);
            }

            return true;
        }
    }
}
