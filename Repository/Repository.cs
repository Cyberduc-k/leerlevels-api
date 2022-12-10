using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository;

public abstract class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
{
    private readonly DbContext _context;
    protected readonly DbSet<TEntity> _dbset;

    protected Repository(DbContext context, DbSet<TEntity> dbset)
    {
        _context = context;
        _dbset = dbset;
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await _dbset.FindAsync(id);
    }

    public async Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dbset.FirstOrDefaultAsync(filter);
    }

    public IAsyncEnumerable<TEntity> GetAllAsync()
    {
        return _dbset.AsAsyncEnumerable();
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbset.AnyAsync(predicate);
    }

    public async Task InsertAsync(TEntity entity)
    {
        await _dbset.AddAsync(entity);
    }

    public void Remove(TEntity entity)
    {
        _dbset.Remove(entity);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _dbset.CountAsync();
    }

    public IQueryableRepository<TEntity> Limit(int limit)
    {
        return new QueryableRepository<TEntity>(_dbset.Take(limit));
    }

    public IQueryableRepository<TEntity> Skip(int count)
    {
        return new QueryableRepository<TEntity>(_dbset.Skip(count));
    }

    public IQueryableRepository<TEntity> OrderBy<TField>(Expression<Func<TEntity, TField>> field)
    {
        return new QueryableRepository<TEntity>(_dbset.OrderBy(field));
    }

    public IQueryableRepository<TEntity> Where(Expression<Func<TEntity, bool>> filter)
    {
        return new QueryableRepository<TEntity>(_dbset.Where(filter));
    }

    public IQueryableRepository<TNew> Select<TNew>(Expression<Func<TEntity, TNew>> selector)
    {
        return new QueryableRepository<TNew>(_dbset.Select(selector));
    }

    public IIncludableRepository<TEntity, TProp> Include<TProp>(Expression<Func<TEntity, TProp>> property)
    {
        return new IncludableRepository<TEntity, TProp>(_dbset.Include(property));
    }

    public IIncludableRepository<TEntity, TProp> Include<TProp>(Expression<Func<TEntity, IEnumerable<TProp>>> property)
    {
        return new ThenIncludableRepository<TEntity, TProp>(_dbset.Include(property));
    }

    public IIncludableRepository<TEntity, TProp> Include<TProp>(Expression<Func<TEntity, ICollection<TProp>>> property)
    {
        return new ThenIncludableRepository<TEntity, TProp>(_dbset.Include(property));
    }
}
