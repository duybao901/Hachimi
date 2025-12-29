using Microsoft.EntityFrameworkCore;
using ProjectionWorker.ReadEntity;

namespace ProjectionWorker;
public class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options)
    : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder builder)
     => builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

    // Entities
    public DbSet<PostReadEntity> Posts => Set<PostReadEntity>();
    public DbSet<TagReadEntity> Tags => Set<TagReadEntity>();
    public DbSet<PostTagReadEntity> PostTags => Set<PostTagReadEntity>();

}
