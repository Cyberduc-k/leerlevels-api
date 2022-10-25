using Model;

namespace Repository.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetUserByLoginInfo(string userName);
}
