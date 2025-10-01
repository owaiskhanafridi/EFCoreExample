using EFCoreExample.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreExample.Infrastructure
{
    public class AmazonDbContext: DbContext
    {
        public AmazonDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Item> Items => Set<Item>();

        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);
            mb.ApplyConfigurationsFromAssembly(typeof(AmazonDbContext).Assembly);
        }
    }
}
