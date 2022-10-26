using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository;

public abstract class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
{
    private readonly DbContext _context;
    protected readonly DbSet<TEntity> _dbset;

    protected Repository(DbContext context, DbSet<TEntity> dbset)
    {
        _context = context;
        _dbset = dbset;
    }

    public IAsyncEnumerable<TEntity> GetAllAsync()
    {
        return _dbset.AsAsyncEnumerable();
    }

    public IAsyncEnumerable<TEntity> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] included)
    {
        IQueryable<TEntity> query = _dbset.AsQueryable();

        foreach (Expression<Func<TEntity, object>> include in included)
            query = query.Include(include);

        return query.AsAsyncEnumerable();
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await _dbset.FindAsync(id);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbset.AnyAsync(predicate);
    }

    public async Task InsertAsync(TEntity entity)
    {
        await _dbset.AddAsync(entity);
    }

    public void Remove(TEntity entity)
    {
        _dbset.Remove(entity);
    }

    public Task SaveChanges()
    {
        return _context.SaveChangesAsync();
    }
}
