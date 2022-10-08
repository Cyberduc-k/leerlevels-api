using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Service.Interfaces;
public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();

    Task<User> GetByIdAsync(string userId);

    Task InsertAsync(User user);

    // Task UpdateAsync(string userId);

    Task DeleteAsync(string userId);

    Task SaveAsync();
}
