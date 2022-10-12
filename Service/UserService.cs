using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service;

public class UserService : IUserService
{
    private readonly ILogger _logger;
    private readonly IUserRepository _userRepository;

    public UserService(ILoggerFactory loggerFactory, IUserRepository userRepository)
    {
        _logger = loggerFactory.CreateLogger<UserService>();
        _userRepository = userRepository;
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
        User user = await GetUserByIdAsync(userId);

        user.Email = userDTO.Email;
        user.UserName = userDTO.UserName;
        user.FirstName = userDTO.FirstName;
        user.LastName = userDTO.LastName;

        //user.Password = userDTO.Password;
        //user.Role = userDTO.Role;

        await _userRepository.SaveChanges();
        return user;
    }

    //delete (soft)
    public async Task DeleteUserAsync(string userId)
    {
        User user = await GetUserByIdAsync(userId);
        user.IsActive = false;
        await _userRepository.SaveChanges(); //update user.IsActive to false;

        _logger.LogInformation($"delete function soft-deleted user {user.UserName} with id: {user.Id}");
    }
}
