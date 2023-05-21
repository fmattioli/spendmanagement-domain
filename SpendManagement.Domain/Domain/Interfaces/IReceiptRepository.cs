using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Interfaces
{
    public interface IReceiptRepository : IBaseRepository<ReceiptDomain>
    {
        Task<bool> AddReceiptItem(Guid receiptId, IEnumerable<ReceiptItemDomain> receiptItems);
    }
}
