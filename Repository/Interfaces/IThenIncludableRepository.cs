using System.Linq.Expressions;

namespace Repository.Interfaces;

public interface IThenIncludableRepository<TEntity, TProp>
{
    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, TNew>> property);
    public IThenIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, IEnumerable<TNew>>> property);
    public IThenIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, ICollection<TNew>>> property);
    public IIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, TNew>> property);
    public IThenIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, IEnumerable<TNew>>> property);
    public IThenIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, ICollection<TNew>>> property);

    public Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> filter);
    public IAsyncEnumerable<TEntity> GetAllAsync();
    public IAsyncEnumerable<TEntity> GetAllWhereAsync(Expression<Func<TEntity, bool>> filter);
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    public IQueryableRepository<TEntity> Limit(int limit);
    public IQueryableRepository<TEntity> Skip(int count);
    public IQueryableRepository<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> field);
}
