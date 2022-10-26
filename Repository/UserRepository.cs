using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Interfaces;

namespace Repository;

public class UserRepository : Repository<User, string>, IUserRepository
{
    public DbSet<User> Users;

    public UserRepository(DataContext context) : base(context, context.Users)
    {
        Users = context.Users;
    }

    public async Task<User?> GetUserByLoginInfo(string email)
    {
        IQueryable<User> query = Users.Where(x => x.Email == email);
        // This is allowed to return a default value (null) when no user is found, so don't alter this to be non-nullable with an ! please, thank you ;)
        return await query.FirstOrDefaultAsync();
    }
}
