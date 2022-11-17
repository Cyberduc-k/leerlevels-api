using System.Linq.Expressions;

namespace Repository.Interfaces;

public interface IQueryableRepository<TEntity>
{
    public Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> filter);
    public IAsyncEnumerable<TEntity> GetAllAsync();
    public IAsyncEnumerable<TEntity> GetAllWhereAsync(Expression<Func<TEntity, bool>> filter);
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    public IQueryableRepository<TEntity> Limit(int limit);
    public IQueryableRepository<TEntity> Skip(int count);
    public IQueryableRepository<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> field);
}
