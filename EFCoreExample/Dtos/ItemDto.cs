//using EFCoreExample.Models;
//using System.Linq;

//namespace EFCoreExample.Dtos
//{
//    public class ItemDto
//    {
//        public Guid Id { get; set; }
//        public DateTime CreatedAt { get; set; }
//        public decimal Amount { get; set; }
//        public decimal Tax { get; set; }
//        public decimal TotalAmount { get; set; }
//        public List<OrderLineDto> Items { get; set; } = new();

//        public static ItemDto FromDomainModel(Order source)
//        {
//            return new OrderDto
//            {
//                Id = source.Id,
//                Amount = source.Amount,
//                Tax = source.Tax,
//                TotalAmount = source.TotalAmount,
//                Items = source.OrderItems.Select(OrderLineDto.FromDomainModel).ToList()
//            };
//        }
//    }

//}
