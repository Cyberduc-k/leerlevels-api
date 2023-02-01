using System.Linq.Expressions;

namespace Repository.Interfaces;

public interface IQueryableRepository<TEntity>
{
    /**
     * <summary>
     * Get a single entity that matches the <paramref name="filter"/>.
     * </summary>
     */
    public Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> filter);

    /**
     * <summary>
     * Get all entities.
     * </summary>
     */
    public IAsyncEnumerable<TEntity> GetAllAsync();

    /**
     * <summary>
     * Check if there are any entities that match the <paramref name="predicate"/>.
     * </summary>
     */
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    /**
     * <summary>
     * Count all entities.
     * </summary>
     */
    public Task<int> CountAsync();

    /**
     * <summary>
     * Limit the number of entities returned.
     * </summary>
     */
    public IQueryableRepository<TEntity> Limit(int limit);

    /**
     * <summary>
     * Skip the first <paramref name="count"/> entities.
     * </summary>
     */
    public IQueryableRepository<TEntity> Skip(int count);

    /**
     * <summary>
     * Order the entities by the given <paramref name="field"/>.
     * </summary>
     */
    public IQueryableRepository<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> field);

    /**
     * <summary>
     * Filter all entities.
     * </summary>
     */
    public IQueryableRepository<TEntity> Where(Expression<Func<TEntity, bool>> filter);

    /**
     * <summary>
     * Get only the selected fields from the entity.
     * </summary>
     */
    public IQueryableRepository<TNew> Select<TNew>(Expression<Func<TEntity, TNew>> selector);
}
