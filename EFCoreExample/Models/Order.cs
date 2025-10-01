using Microsoft.AspNetCore.Http.Features;

namespace EFCoreExample.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal Tax { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = [];



        //public Customer Customer { get; set; }

        //public Address Address { get; set; }
    }
}
