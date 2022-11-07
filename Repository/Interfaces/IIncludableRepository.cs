using System.Linq.Expressions;

namespace Repository.Interfaces;

public interface IIncludableRepository<TEntity, TProp>
{
    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, TNew>> property);
    public IThenIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, IEnumerable<TNew>>> property);
    public IThenIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, ICollection<TNew>>> property);

    public Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> filter);
    public IAsyncEnumerable<TEntity> GetAllAsync();
    public IAsyncEnumerable<TEntity> GetAllWhereAsync(Expression<Func<TEntity, bool>> filter);
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
}
