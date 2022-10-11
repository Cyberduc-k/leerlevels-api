namespace Repository.Interfaces;

public interface IRepository<T> where T : class
{
    public IAsyncEnumerable<T> GetAllAsync();
    public Task<T?> GetByIdAsync(string id);
    public Task InsertAsync(T entity);
    public Task RemoveAsync(string id);
    public Task SaveChanges();
}
