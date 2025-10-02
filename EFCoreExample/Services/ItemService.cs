using EFCoreExample.Infrastructure;
using EFCoreExample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreExample.Services
{
    //private readonly IHistoryRepository<Order> _bundles;
    public class ItemService
    {
        //We dont need separate uow anymore since the dbContext already provides one
        private readonly IUnitOfWork _uow;
        private readonly AmazonDbContext _dbContext;

        public ItemService(IUnitOfWork uow, AmazonDbContext dbContext)
        {
            _uow = uow;
            _dbContext = dbContext;
        }

        //TODO: Create a record dto 'OrderDto' insetad of using actual entity model Order
        public async Task<Item> CreateItemAsync(Item itemDto, CancellationToken ct)
        {
            var entity = new Order
            {
                //TODO: after creating an actual OrderDto, Convert dto to model here
            };

            await _dbContext.Items.AddAsync(itemDto);
            await _dbContext.SaveChangesAsync();
            //await _uow.SaveChangesAsync(ct);
            return itemDto;
        }

        public Task<Item?> GetItemAsync(Guid id, CancellationToken ct)
            => _dbContext.Items.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id, ct);

        public Task<List<Item>> GetAllItemAsync(CancellationToken ct)
            => _dbContext.Items.AsNoTracking().ToListAsync(ct);
    }

}
