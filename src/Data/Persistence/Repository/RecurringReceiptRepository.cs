using Domain.Entities;
using Domain.Interfaces;
using MongoDB.Driver;
using Serilog;

namespace Data.Persistence.Repository
{
    public class RecurringReceiptRepository(IMongoDatabase mongoDb, ILogger _logger) : BaseRepository<RecurringReceiptEntity>(mongoDb, "RecurringReceipts", _logger), IRecurringReceiptRepository
    {
    }
}
