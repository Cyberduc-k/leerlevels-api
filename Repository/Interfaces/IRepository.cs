using System.Linq.Expressions;

namespace Repository.Interfaces;

public interface IRepository<TEntity, TId> : IQueryableRepository<TEntity> where TEntity : class
{
    /**
     * <summary>
     * Get the entity with the given id.
     * </summary>
     */
    public Task<TEntity?> GetByIdAsync(TId id);

    /**
     * <summary>
     * Insert a new entity.
     * </summary>
     */
    public Task InsertAsync(TEntity entity);

    /**
     * <summary>
     * Remove the given entity.
     * </summary>
     */
    public void Remove(TEntity entity);

    /**
     * <summary>
     * Save any changes to the database. If this method is not called, the database remains unchanged.
     * </summary>
     */
    public Task SaveChanges();

    /**
     * <summary>
     * Include a child entity when querying data.
     * </summary>
     */
    public IIncludableRepository<TEntity, TProp> Include<TProp>(Expression<Func<TEntity, TProp>> property);

    /**
     * <summary>
     * Include all child entities when querying data.
     * </summary>
     */
    public IIncludableRepository<TEntity, TProp> Include<TProp>(Expression<Func<TEntity, IEnumerable<TProp>>> property);

    /**
     * <summary>
     * Include all child entities when querying data.
     * </summary>
     */
    public IIncludableRepository<TEntity, TProp> Include<TProp>(Expression<Func<TEntity, ICollection<TProp>>> property);
}
