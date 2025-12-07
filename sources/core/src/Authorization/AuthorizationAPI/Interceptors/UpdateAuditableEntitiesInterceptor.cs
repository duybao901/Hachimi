//using AuthorizationAPI.Abstractions.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using Microsoft.EntityFrameworkCore.Diagnostics;

//namespace AuthorizationAPI.Interceptors;

//public sealed class UpdateAuditableEntitiesInterceptor
//    : SaveChangesInterceptor
//{
//    // Call before Saving
//    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
//        DbContextEventData eventData,
//        InterceptionResult<int> result,
//        CancellationToken cancellationToken = default)
//    {
//        DbContext? dbContext = eventData.Context;

//        if (dbContext == null)
//        {
//            return base.SavingChangesAsync(eventData, result, cancellationToken);
//        }
//        IEnumerable<EntityEntry<IAuditTableEntity>> entries = dbContext.ChangeTracker.Entries<IAuditTableEntity>();

//        foreach (EntityEntry<IAuditTableEntity> entityEntry in entries)
//        {
//            if (entityEntry.State == EntityState.Added)
//            {
//                // !TODO: Get user Added
//                entityEntry.Property(a => a.CreatedOnUtc).CurrentValue = DateTime.UtcNow;
//            }

//            if (entityEntry.State == EntityState.Modified)
//            {
//                // !TODO: Get user Modified
//                entityEntry.Property(a => a.ModifiedOnUtc).CurrentValue = DateTime.UtcNow;
//            }
//        }

//        return base.SavingChangesAsync(eventData, result, cancellationToken);
//    }
//}
