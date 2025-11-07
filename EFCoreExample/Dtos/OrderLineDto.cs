using EFCoreExample.Models;

namespace EFCoreExample.Dtos
{
    public class OrderLineDto
    {
        public Guid ItemId { get; set; }
        public string Title { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public static OrderLineDto FromDomainModel(OrderItem source)
        {
            return new OrderLineDto
            {
                ItemId = source.ItemId,
                Quantity = source.Quantity,
                UnitPrice = source.UnitPrice,
                Title = source.Item.Title
            };
        }
    }
}

