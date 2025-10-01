namespace EFCoreExample.Models
{
    public class OrderItem
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
