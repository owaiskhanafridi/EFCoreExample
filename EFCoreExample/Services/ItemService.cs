using EFCoreExample.Commands;
using EFCoreExample.Exceptions;
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
        public async Task<Item> CreateItemAsync(CreateItemCommand itemDto, CancellationToken ct)
        {
            var itemExists = _dbContext.Items.SingleOrDefault(x => x.Title == itemDto.title);

            if (itemExists != null)
                throw new DuplicateResourceException( $"Item with title {itemDto.title} already exist. " +
                    $"Please select a different title name");
            if (itemDto.price < 0)
                throw new NegativePriceException("Price cannot be lower than zero");

            var entity = new Item
            {
                Title = itemDto.title,
                Price = itemDto.price,
                CreatedAt = itemDto.createdAt
                
                //TODO: after creating an actual OrderDto, Convert dto to model here
            };

            await _dbContext.Items.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            //await _uow.SaveChangesAsync(ct);
            return entity;
        }

        public Task<Item?> GetItemAsync(Guid id)
            => _dbContext.Items.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);

        public Task<Item?> GetItemByTitleAsync(string title)
    => _dbContext.Items.AsNoTracking().FirstOrDefaultAsync(o => o.Title == title);


        public Task<List<Item>> GetAllItemAsync(CancellationToken ct)
            => _dbContext.Items.AsNoTracking().ToListAsync(ct);

        public Task<List<Item>> GetItemsAddedAfter(DateTime date)
            => _dbContext.Items.AsNoTracking().Where(x => x.CreatedAt >= date).ToListAsync();
    }

}
