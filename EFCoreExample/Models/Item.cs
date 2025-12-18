namespace EFCoreExample.Models
{
    public class Item
    {
        /// <summary>
        /// Unique Identified for Item    
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Name of the item
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Price of the item
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Order Items (can it be described here ?)
        /// </summary>
        public ICollection<OrderItem> OrderItems { get; set; } = [];

    }
}
