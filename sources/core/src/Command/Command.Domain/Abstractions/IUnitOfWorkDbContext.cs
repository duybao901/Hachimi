using Microsoft.EntityFrameworkCore;

namespace Command.Domain.Abstractions;
public interface IUnitOfWorkDbContext<TContext> : IAsyncDisposable 
    where TContext : DbContext
{
    /// <summary>
    /// Call save change from db context
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
