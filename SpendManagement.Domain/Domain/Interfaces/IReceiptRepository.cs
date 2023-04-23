using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Interfaces
{
    public interface IReceiptRepository : IBaseRepository<Receipt>
    {
        Task<bool> AddReceiptItem(Guid receiptId, IEnumerable<ReceiptItem> receiptItems);
    }
}
