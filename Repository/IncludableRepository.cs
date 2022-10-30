using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repository.Interfaces;

namespace Repository;

public class IncludableRepository<TEntity, TProp, TColl> : IIncludableRepository<TEntity, TProp, TColl> where TEntity : class
{
    private readonly IIncludableQueryable<TEntity, TColl> _query;
    IIncludableQueryable<TEntity, TColl> IIncludableRepository<TEntity, TProp, TColl>.Queryable => _query;

    internal IncludableRepository(IIncludableQueryable<TEntity, TColl> query)
    {
        _query = query;
    }

    public IIncludableRepository<TEntity, TNew, TNew> Include<TNew>(Expression<Func<TEntity, TNew>> property)
    {
        return new IncludableRepository<TEntity, TNew, TNew>(_query.Include(property));
    }

    public IIncludableRepository<TEntity, TNew, IEnumerable<TNew>> Include<TNew>(Expression<Func<TEntity, IEnumerable<TNew>>> property)
    {
        return new IncludableRepository<TEntity, TNew, IEnumerable<TNew>>(_query.Include(property));
    }

    public IIncludableRepository<TEntity, TNew, IEnumerable<TNew>> Include<TNew>(Expression<Func<TEntity, ICollection<TNew>>> property)
    {
        return new IncludableRepository<TEntity, TNew, IEnumerable<TNew>>(_query.Include(property));
    }

    public IIncludableRepository<TEntity, TNew, TNew> ThenInclude<TNew>(Expression<Func<TProp, TNew>> property)
    {
        if (_query is IIncludableQueryable<TEntity, IEnumerable<TProp>> query)
            return new IncludableRepository<TEntity, TNew, TNew>(query.ThenInclude(property));
        throw new InvalidOperationException();
    }

    public IIncludableRepository<TEntity, TNew, IEnumerable<TNew>> ThenInclude<TNew>(Expression<Func<TProp, IEnumerable<TNew>>> property)
    {
        if (_query is IIncludableQueryable<TEntity, IEnumerable<TProp>> query)
            return new IncludableRepository<TEntity, TNew, IEnumerable<TNew>>(query.ThenInclude(property));
        throw new InvalidOperationException();
    }

    public IIncludableRepository<TEntity, TNew, IEnumerable<TNew>> ThenInclude<TNew>(Expression<Func<TProp, ICollection<TNew>>> property)
    {
        if (_query is IIncludableQueryable<TEntity, IEnumerable<TProp>> query)
            return new IncludableRepository<TEntity, TNew, IEnumerable<TNew>>(query.ThenInclude(property));
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
}
