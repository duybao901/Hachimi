using AuthorizationAPI.Abstractions.Entities;
using AuthorizationAPI.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthorizationAPI.Repositories;
public class RepositoryBase<TEntity, Tkey> : IRepositoryBase<TEntity, Tkey> where TEntity : DomainEntity<Tkey>
{
    private readonly ApplicationDbContext _context;

    public RepositoryBase(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        // read-only: Important Always include AsNoTracking for Query Side
        IQueryable<TEntity> items = _context.Set<TEntity>().AsNoTracking();
        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties)
                items = items.Include(includeProperty);
        }

        if (predicate != null)
        {
            items = items.Where(predicate);
        }

        return items;
    }

    public async Task<TEntity> FindByIdAsync(Tkey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await FindAll(null, includeProperties).AsTracking().SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public async Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await FindAll(null, includeProperties).AsTracking().SingleOrDefaultAsync(predicate, cancellationToken);
    }

    // Working side by side with Unit of Work, this method just adds change status and is not in the IO-Bound
    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    // Working side by side with Unit of Work, this method just adds change status and is not in the IO-Bound
    public void Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    // Working side by side with Unit of Work, this method just adds change status and is not in the IO-Bound
    public void RemoveMultiple(List<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
    }

    // Working side by side with Unit of Work, this method just adds change status and is not in the IO-Bound
    public void Update(TEntity entity)
    {
        // Set the entity state to modified
        _context.Set<TEntity>().Update(entity);
    }
}
