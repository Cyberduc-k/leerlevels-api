using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class UserRepository : IUserRepository
{
    private readonly TargetContext targetContext;

    public UserRepository(TargetContext targetContext)
    {
        this.targetContext = targetContext;
    }

    public Task DeleteAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByIdAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task InsertAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync()
    {
        throw new NotImplementedException();
    }
}
