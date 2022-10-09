using Model;

namespace Repository.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();

    Task<User> GetByIdAsync(string userId);

    Task InsertAsync(User user);

    // Task UpdateAsync(string userId);

    Task DeleteAsync(string userId);

    Task SaveAsync();
}
