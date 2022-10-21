using System.Linq.Expressions;

namespace Repository.Interfaces;

public interface IRepository<T> where T : class
{
    public IAsyncEnumerable<T> GetAllAsync();
    public IAsyncEnumerable<T> GetAllIncludingAsync(params Expression<Func<T, object>>[] included);
    public Task<T?> GetByIdAsync(string id);
    public Task InsertAsync(T entity);
    public Task RemoveAsync(string id);
    public Task SaveChanges();

    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
}
