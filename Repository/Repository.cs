using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository;

public abstract class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbset;

    protected Repository(DbContext context, DbSet<T> dbset)
    {
        _context = context;
        _dbset = dbset;
    }

    public IAsyncEnumerable<T> GetAllAsync()
    {
        return _dbset.AsAsyncEnumerable();
    }

    public IAsyncEnumerable<T> GetAllIncludingAsync(params Expression<Func<T, object>>[] included)
    {
        IQueryable<T> query = _dbset.AsQueryable();

        foreach (Expression<Func<T, object>> include in included)
            query = query.Include(include);

        return query.AsAsyncEnumerable();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        return await _dbset.FindAsync(id);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbset.AnyAsync(predicate);
    }

    public async Task InsertAsync(T entity)
    {
        await _dbset.AddAsync(entity);
    }

    public async Task RemoveAsync(string id)
    {
        T? entity = await _dbset.FindAsync(id);
        _dbset.Remove(entity!);
    }

    public Task SaveChanges()
    {
        return _context.SaveChangesAsync();
    }
}
