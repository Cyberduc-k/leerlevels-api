using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Repository.Interfaces;

public interface IIncludableRepository<TEntity, TProp>
{
    protected IIncludableQueryable<TEntity, IEnumerable<TProp>> Queryable { get; }

    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, IEnumerable<TNew>>> property);
    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, ICollection<TNew>>> property);
    public IIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, IEnumerable<TNew>>> property);
    public IIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, ICollection<TNew>>> property);

    public Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> filter);
    public IAsyncEnumerable<TEntity> GetAllAsync();
    public IAsyncEnumerable<TEntity> GetAllWhereAsync(Expression<Func<TEntity, bool>> filter);
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
}
