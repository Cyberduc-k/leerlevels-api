using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Repository.Interfaces;
using Service.Exceptions;
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
    public async Task<User> UpdateUser(string userId, UpdateUserDTO changes)
    {
        User user = await GetUserById(userId) ?? throw new NotFoundException("user to update");

        user.Email = changes.Email ?? user.Email;
        user.FirstName = changes.FirstName ?? user.FirstName;
        user.LastName = changes.LastName ?? user.LastName;
        user.UserName = changes.UserName ?? user.UserName;

        //user.Password = changes.Password ?? user.Password;
        //user.Role = changes.Role ?? user.Role;

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
