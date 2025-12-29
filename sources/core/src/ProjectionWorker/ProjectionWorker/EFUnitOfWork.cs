using ProjectionWorker.Abstractions;

namespace ProjectionWorker;
public class EFUnitOfWork : IUnitOfWork
{
    private readonly ReadDbContext _context;

    public EFUnitOfWork(ReadDbContext context)
        => _context = context;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync();

    async ValueTask IAsyncDisposable.DisposeAsync()
        => await _context.DisposeAsync();
}