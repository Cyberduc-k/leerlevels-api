using System.Linq.Expressions;

namespace Repository.Interfaces;

public interface IRepository<TEntity, TId> : IQueryableRepository<TEntity> where TEntity : class
{
    public Task<TEntity?> GetByIdAsync(TId id);

    public Task InsertAsync(TEntity entity);
    public void Remove(TEntity entity);
    public Task SaveChanges();

    public IIncludableRepository<TEntity, TProp> Include<TProp>(Expression<Func<TEntity, TProp>> property);
    public IIncludableRepository<TEntity, TProp> Include<TProp>(Expression<Func<TEntity, IEnumerable<TProp>>> property);
    public IIncludableRepository<TEntity, TProp> Include<TProp>(Expression<Func<TEntity, ICollection<TProp>>> property);
}
