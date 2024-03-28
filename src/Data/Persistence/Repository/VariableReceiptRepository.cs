using Domain.Entities;
using Domain.Interfaces;
using MongoDB.Driver;
using Serilog;

namespace Data.Persistence.Repository
{
    public class VariableReceiptRepository(IMongoDatabase mongoDb, ILogger _logger) : BaseRepository<ReceiptEntity>(mongoDb, "Receipts", _logger), IVariableReceiptRepository
    {
    }
}
