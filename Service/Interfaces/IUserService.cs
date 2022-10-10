using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface IUserService
{
    public Task<ICollection<User>> GetUsersAsync();

    Task<User> GetUserByIdAsync(string userId);

    Task<User> CreateUserAsync(User user);

    Task<User> UpdateUserAsync(string userId, UserDTO userDTO);

    Task DeleteUserAsync(string userId);

    //Task<ICollection<Group>> UpdateUserGroupsAsync();

    //Task<ICollection<Set>> UpdateUserSetsAsync();
}
