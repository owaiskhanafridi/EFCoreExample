using EFCoreExample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreExample.Infrastructure.EntityFramework.Configurations
{
    public class ItemConfiguration: IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Price).HasPrecision(5, 2);
            
            //This is option since the property 'title' is already declared as string (without ?) in the model.
            //EF Core will already treat this column as required without the following line
            entityBuilder.Property(x => x.Title).IsRequired();

            entityBuilder.HasMany(x => x.OrderItems)
                .WithOne(x => x.Item)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
