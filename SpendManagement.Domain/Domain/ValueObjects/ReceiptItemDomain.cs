using Domain.Entities;

namespace Domain.ValueObjects
{
    public record ReceiptItemDomain
    {
        public Guid Id { get; set; }
        public Guid ReceiptId { get; set; }
        public Guid CategoryId { get; set; }
        public string ItemName { get; set; } = null!;
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalPrice { get { return Quantity * ItemPrice; } }
        public string Observation { get; set; } = null!;
    }
}
