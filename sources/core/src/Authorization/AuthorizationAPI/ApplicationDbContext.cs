using AuthorizationApi;
using AuthorizationAPI.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Action = AuthorizationAPI.Identity.Action;

namespace AuthorizationAPI;
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
}
