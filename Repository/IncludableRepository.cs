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

    public IThenIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, IEnumerable<TNew>>> property)
    {
        return new ThenIncludableRepository<TEntity, TNew>(_query.Include(property));
    }

    public IThenIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, ICollection<TNew>>> property)
    {
        return new ThenIncludableRepository<TEntity, TNew>(_query.Include(property));
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
}
