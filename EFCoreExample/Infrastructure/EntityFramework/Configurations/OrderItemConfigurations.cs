using EFCoreExample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreExample.Infrastructure.EntityFramework.Configurations
{
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> entityBuilder)
        {
            entityBuilder.ToTable("OrderItems");
            entityBuilder.HasKey(x => new { x.OrderId, x.ItemId });
            entityBuilder.Property(x => x.UnitPrice).HasPrecision(5, 2);
            entityBuilder.Property(x => x.Quantity).HasDefaultValue(1);
        }
    }
}
