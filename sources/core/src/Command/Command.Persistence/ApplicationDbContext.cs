using Command.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Command.Persistence;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
     => builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

    // Entities
    public DbSet<Post> Posts { get; set; }
}
