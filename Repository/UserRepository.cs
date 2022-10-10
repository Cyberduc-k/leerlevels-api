using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(UserContext context) : base (context, context.Users)
    {
    }
}
