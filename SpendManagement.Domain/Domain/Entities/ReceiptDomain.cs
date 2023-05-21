using Domain.ValueObjects;

namespace Domain.Entities
{
    public class ReceiptDomain
    {
        public ReceiptDomain(Guid id, string establishmentName, DateTime receiptDate, IEnumerable<ReceiptItemDomain> receiptItems)
        {
            Id = id;
            EstablishmentName = establishmentName;
            ReceiptDate = receiptDate;
            ReceiptItems = receiptItems;
        }

        public Guid Id { get; set; }
        public string EstablishmentName { get; set; }
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItemDomain> ReceiptItems { get; set; }
    }
}
