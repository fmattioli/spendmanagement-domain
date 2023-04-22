using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Receipt
    {
        public Receipt(Guid id, string establishmentName, DateTime receiptDate, IEnumerable<ReceiptItem> receiptItems)
        {
            Id = id;
            EstablishmentName = establishmentName;
            ReceiptDate = receiptDate;
            ReceiptItems = receiptItems;
        }

        public Guid Id { get; set; }
        public string EstablishmentName { get; set; }
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItem> ReceiptItems { get; set; }
    }
}
