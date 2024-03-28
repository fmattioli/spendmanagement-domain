using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class ReceiptEntity(Guid id, Guid categoryId, string establishmentName, DateTime receiptDate, IEnumerable<ReceiptItemEntity> receiptItems, decimal discount, decimal total)
    {
        [BsonId]
        public Guid Id { get; set; } = id;
        public Guid CategoryId { get; set; } = categoryId;
        public string EstablishmentName { get; set; } = establishmentName;
        public DateTime ReceiptDate { get; set; } = receiptDate;
        public IEnumerable<ReceiptItemEntity> ReceiptItems { get; set; } = receiptItems;
        public decimal Discount { get; set; } = discount;
        public decimal Total { get; set; } = total;
    }

    public class ReceiptItemEntity
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; } = null!;
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal ItemDiscount { get; set; }
        public string Observation { get; set; } = null!;
    }
}
