using Data.Session;
using Domain.Entities;
using Domain.Interfaces;

namespace Data.Persistence
{
    public class ReceiptRepository : BaseRepository<Receipt>, IReceiptRepository
    {
        public ReceiptRepository(DbSession dbSession) : base(dbSession)
        {
        }
    }
}
