using System.Linq.Expressions;

namespace Repository.Interfaces;

public interface IRepository<TEntity, TId> where TEntity : class
{
    public IQueryable<TEntity> GetAllAsync();
    public IQueryable<TEntity> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] included);
    public Task<TEntity?> GetByIdAsync(TId id);
    public Task InsertAsync(TEntity entity);
    public void Remove(TEntity entity);
    public Task SaveChanges();

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
}
