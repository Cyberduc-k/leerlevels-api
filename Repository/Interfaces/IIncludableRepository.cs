using System.Linq.Expressions;

namespace Repository.Interfaces;

public interface IIncludableRepository<TEntity, TProp> : IQueryableRepository<TEntity>
{
    /**
     * <summary>
     * Include a child entity when querying data.
     * </summary>
     */
    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, TNew>> property);

    /**
     * <summary>
     * Include all child entities when querying data.
     * </summary>
     */
    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, IEnumerable<TNew>>> property);

    /**
     * <summary>
     * Include all child entities when querying data.
     * </summary>
     */
    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, ICollection<TNew>>> property);

    /**
     * <summary>
     * Include a child entity on the previously included child when querying data.
     * </summary>
     */
    public IIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, TNew>> property);

    /**
     * <summary>
     * Include all child entities on the previously included child when querying data.
     * </summary>
     */
    public IIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, IEnumerable<TNew>>> property);

    /**
     * <summary>
     * Include all child entities on the previously included child when querying data.
     * </summary>
     */
    public IIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, ICollection<TNew>>> property);
}
