namespace EFCoreExample.Infrastructure
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken ct);
    }
}
