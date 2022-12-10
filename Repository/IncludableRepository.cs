using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repository.Interfaces;

namespace Repository;

public class IncludableRepository<TEntity, TProp> : IIncludableRepository<TEntity, TProp> where TEntity : class
{
    private readonly IIncludableQueryable<TEntity, TProp> _query;

    internal IncludableRepository(IIncludableQueryable<TEntity, TProp> query)
    {
        _query = query;
    }

    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, TNew>> property)
    {
        return new IncludableRepository<TEntity, TNew>(_query.Include(property));
    }

    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, IEnumerable<TNew>>> property)
    {
        return new ThenIncludableRepository<TEntity, TNew>(_query.Include(property));
    }

    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, ICollection<TNew>>> property)
    {
        return new ThenIncludableRepository<TEntity, TNew>(_query.Include(property));
    }

    public IIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, TNew>> property)
    {
        throw new InvalidOperationException();
    }

    public IIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, IEnumerable<TNew>>> property)
    {
        throw new InvalidOperationException();
    }

    public IIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, ICollection<TNew>>> property)
    {
        throw new InvalidOperationException();
    }

    public Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> filter)
    {
        return _query.FirstOrDefaultAsync(filter);
    }

    public IAsyncEnumerable<TEntity> GetAllAsync()
    {
        return _query.AsAsyncEnumerable();
    }

    public IAsyncEnumerable<TEntity> GetAllWhereAsync(Expression<Func<TEntity, bool>> filter)
    {
        return _query.Where(filter).AsAsyncEnumerable();
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return _query.AnyAsync(predicate);
    }

    public async Task<int> CountAsync()
    {
        return await _query.CountAsync();
    }

    public IQueryableRepository<TEntity> Limit(int limit)
    {
        return new QueryableRepository<TEntity>(_query.Take(limit));
    }

    public IQueryableRepository<TEntity> Skip(int count)
    {
        return new QueryableRepository<TEntity>(_query.Skip(count));
    }

    public IQueryableRepository<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> field)
    {
        return new QueryableRepository<TEntity>(_query.OrderBy(field));
    }

    public IQueryableRepository<TEntity> Where(Expression<Func<TEntity, bool>> filter)
    {
        return new QueryableRepository<TEntity>(_query.Where(filter));
    }

    public IQueryableRepository<TNew> Select<TNew>(Expression<Func<TEntity, TNew>> selector)
    {
        return new QueryableRepository<TNew>(_query.Select(selector));
    }
}
