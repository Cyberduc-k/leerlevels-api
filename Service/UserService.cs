using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    private ILogger _Logger { get; }

    public UserService(IUserRepository userRepository, ILogger logger)
    {
        _userRepository = userRepository;
        _Logger = logger;
    }

    //get users
    public async Task<ICollection<User>> GetUsersAsync()
    {
        return await _userRepository.GetAllAsync().ToArrayAsync();
    }

    //get user
    public async Task<User> GetUserByIdAsync(string userId)
    {
        return await _userRepository.GetByIdAsync(userId) ?? throw new NullReferenceException();
    }

    //create
    public async Task<User> CreateUserAsync(User user)
    {
        user.Id = Guid.NewGuid().ToString();
        await _userRepository.InsertAsync(user);
        await _userRepository.SaveChanges();
        return user;
    }

    //update
    public async Task<User> UpdateUserAsync(string userId, UserDTO userDTO)
    {
        User user = await this.GetUserByIdAsync(userId);

        user.Email = userDTO.Email;
        user.UserName = userDTO.UserName;
        user.FirstName = userDTO.FirstName;
        user.LastName = userDTO.LastName;

        //user.Password = userDTO.Password;
        //user.Role = userDTO.Role;

        _userRepository.UpdateAsync(user);
        return user;
    }

    //delete (soft)
    public async Task DeleteUserAsync(string userId)
    {
        User user = await this.GetUserByIdAsync(userId);
        user.IsActive = false;
        _userRepository.UpdateAsync(user); //update user.IsActive to false;

        _Logger.LogInformation($"delete function soft-deleted user {user.UserName} with id: {user.Id}");
    }
}
