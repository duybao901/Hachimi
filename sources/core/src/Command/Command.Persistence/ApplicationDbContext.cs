using Command.Domain.Entities;
using Command.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Action = Command.Domain.Entities.Identity.Action;

namespace Command.Persistence;
public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
     => builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

    // Identity
    public DbSet<AppUser> AppUses { get; set; }

    public DbSet<Action> Actions { get; set; }

    public DbSet<Function> Functions { get; set; }

    public DbSet<ActionInFunction> ActionInFunctions { get; set; }

    public DbSet<Permission> Permissions { get; set; }

    // Entities
    public DbSet<Post> Posts { get; set; }
}
