using Data.Session;
using Domain.Entities;

namespace Data.Persistence
{
    internal class ReceiptRepository : BaseRepository<Receipt>
    {
        public ReceiptRepository(DbSession dbSession) : base(dbSession)
        {
        }
    }
}
