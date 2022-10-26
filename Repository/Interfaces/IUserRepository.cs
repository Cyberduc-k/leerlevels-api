using Model;

namespace Repository.Interfaces;

public interface IUserRepository : IRepository<User, string>
{
    Task<User?> GetUserByLoginInfo(string userName);
}
