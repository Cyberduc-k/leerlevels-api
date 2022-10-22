using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Interfaces;

namespace Repository;

public class UserRepository : Repository<User>, IUserRepository
{
    public DbSet<User> Users;

    public UserRepository(DataContext context) : base(context, context.Users)
    {
        Users = context.Users;
    }

    public async Task<User> GetUserByLoginInfo(string userName, string password)
    {
        IQueryable<User> query = Users;
        query = query.Where(x => x.UserName == userName && x.Password == password);
        // This is allowed to return a default value (null) when no user is found, so don't alter this to be non-nullable with an ! please, thank you ;)
        return await query.FirstOrDefaultAsync();
    }
}
