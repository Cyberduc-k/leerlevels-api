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
        return await query.FirstOrDefaultAsync();
    }
}
