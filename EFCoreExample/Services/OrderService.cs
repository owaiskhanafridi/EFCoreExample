using EFCoreExample.Commands;
using EFCoreExample.Dtos;
using EFCoreExample.Exceptions;
using EFCoreExample.Infrastructure;
using EFCoreExample.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreExample.Services
{
    //private readonly IHistoryRepository<Order> _bundles;
    public class OrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly AmazonDbContext _dbContext;
        private readonly ItemService _itemService;

        public OrderService(IUnitOfWork uow, AmazonDbContext dbContext, ItemService itemService)
        {
            _uow = uow;
            _dbContext = dbContext;
            _itemService = itemService;

        }

        //TODO: Create a record dto 'OrderDto' insetad of using actual entity model Order
        public async Task<Guid> CreateOrderAsync(CreateOrderCommand cmd, CancellationToken ct)
        {
            decimal taxRate = 0.01m;
            decimal amount = 0;

            var orderItems = new List<OrderItem>();

            foreach (var item in cmd.OrderItems)
            {
                var itemEntry = _itemService.GetItemAsync(item.ItemId).Result;

                if (itemEntry == null)
                    throw new OrderItemNotFoundException($"Item {item.ItemId} could not be found in the Inventory");

                orderItems.Add(new OrderItem
                {
                    ItemId = itemEntry.Id,
                    OrderId = Guid.Empty,
                    Quantity = item.Quantity,
                    UnitPrice = itemEntry.Price,
                });

                //if (item.Quantity <= 0)
                //    //Throw custom exception

                amount += (item.Quantity * itemEntry.Price);
            }

            var orderEntry = new Order()
            {
                Amount = amount,
                TotalAmount = amount + (taxRate * amount),
                Tax = taxRate,
                OrderItems = orderItems,
            };

            await _dbContext.Orders.AddAsync(orderEntry);
            await _uow.SaveChangesAsync(ct);
            return orderEntry.Id;
        }

        public async Task<OrderDto?> GetOrderAsync(Guid id, CancellationToken ct)
        {
            try
            {
                var response = await _dbContext.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)            // load lines
            .ThenInclude(oi => oi.Item)            // optional: load Item details
            .FirstOrDefaultAsync(o => o.Id == id, ct);

                if (response == null)
                    throw new OrderNotFoundException($"Cannot find the order by OrderId {id}");
                return OrderDto.FromDomainModel(response);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }

}
