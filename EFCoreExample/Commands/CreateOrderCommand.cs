namespace EFCoreExample.Commands
{
    public record CreateOrderCommand(string Notes,
        ICollection<CreateOrderItemCommand> OrderItems);

    public record CreateOrderItemCommand(Guid ItemId, int Quantity);
}
