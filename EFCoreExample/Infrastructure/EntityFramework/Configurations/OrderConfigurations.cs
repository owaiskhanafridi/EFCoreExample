using EFCoreExample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreExample.Infrastructure.EntityFramework.Configurations
{
    public class OrderConfigurations: IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Amount).HasPrecision(18, 2);
            entityBuilder.Property(x => x.TotalAmount).HasPrecision(18, 2);
            entityBuilder.Property(x => x.Tax).HasPrecision(18, 2);
            
            entityBuilder.HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
