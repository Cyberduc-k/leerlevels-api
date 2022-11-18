using System.Linq.Expressions;

namespace Repository.Interfaces;

public interface IIncludableRepository<TEntity, TProp> : IQueryableRepository<TEntity>
{
    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, TNew>> property);
    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, IEnumerable<TNew>>> property);
    public IIncludableRepository<TEntity, TNew> Include<TNew>(Expression<Func<TEntity, ICollection<TNew>>> property);
    public IIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, TNew>> property);
    public IIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, IEnumerable<TNew>>> property);
    public IIncludableRepository<TEntity, TNew> ThenInclude<TNew>(Expression<Func<TProp, ICollection<TNew>>> property);
}
