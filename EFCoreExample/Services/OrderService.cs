using EFCoreExample.Infrastructure;
using EFCoreExample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreExample.Services
{
    //private readonly IHistoryRepository<Order> _bundles;
    public class OrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly AmazonDbContext _dbContext;

        public OrderService(IUnitOfWork uow, AmazonDbContext dbContext)
        {
            _uow = uow;
            _dbContext = dbContext;
        }

        //TODO: Create a record dto 'OrderDto' insetad of using actual entity model Order
        public async Task<Guid> CreateOrderAsync(Order orderDto, CancellationToken ct)
        {
            var entity = new Order
            {
                //TODO: after creating an actual OrderDto, Convert dto to model here
            };

            await _dbContext.Orders.AddAsync(orderDto);
            //await _uow.SaveChangesAsync(ct);
            await _uow.SaveChangesAsync(ct);
            return orderDto.Id;
        }

        public Task<Order?> GetOrderAsync(Guid id, CancellationToken ct)
            => _dbContext.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id, ct);
    }

}
