using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Exceptions;

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

    // get users
    public async Task<ICollection<User>> GetUsers()
    {
        return await _userRepository.GetAllAsync().ToArrayAsync() ?? throw new NotFoundException("users");
    }

    // get user
    public async Task<User> GetUserById(string userId)
    {
        return await _userRepository.GetByIdAsync(userId) ?? throw new NotFoundException("user");
    }

    // create
    public async Task<User> CreateUser(User user)
    {
        user.Id = Guid.NewGuid().ToString();
        await _userRepository.InsertAsync(user);
        await _userRepository.SaveChanges();
        return user;
    }

    // update
    public async Task<User> UpdateUser(string userId, UserDTO userDTO)
    {
        User user = await GetUserById(userId) ?? throw new NotFoundException("user to update");

        user.Email = userDTO.Email;
        user.UserName = userDTO.UserName;
        user.FirstName = userDTO.FirstName;
        user.LastName = userDTO.LastName;

        // user.Password = userDTO.Password;
        // user.Role = userDTO.Role;

        await _userRepository.SaveChanges();
        return user;
    }

    // delete (soft)
    public async Task DeleteUser(string userId)
    {
        // retrieve user
        User user = await GetUserById(userId);

        // update user.IsActive to false;
        user.IsActive = false;
        await _userRepository.SaveChanges(); 

        _logger.LogInformation($"delete function soft-deleted user {user.UserName} with id: {user.Id}");
    }
}
