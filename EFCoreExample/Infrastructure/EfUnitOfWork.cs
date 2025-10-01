namespace EFCoreExample.Infrastructure
{
    public class EfUnitOfWork: IUnitOfWork
    {
        private readonly AmazonDbContext _db;

        public EfUnitOfWork(AmazonDbContext db) => _db = db;

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _db.SaveChangesAsync(ct);

        public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action,
                                               CancellationToken ct = default)
        {
            // If already in a transaction, just run the action
            if (_db.Database.CurrentTransaction is not null)
            {
                await action(ct);
                return;
            }

            // Start a new transaction
            await using var tx = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                await action(ct);

                // Make sure changes are persisted prior to commit
                await _db.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }
    }
}
