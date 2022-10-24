using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Repository;
using Repository.Interfaces;
using Service.Exceptions;
using Service.Interfaces;

namespace Service;

public class UserService : IUserService
{
    private readonly ILogger _logger;
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly ISetRepository _setRepository;

    public UserService(ILoggerFactory loggerFactory, IUserRepository userRepository, IGroupRepository groupRepository, ISetRepository setRepository)
    {
        _logger = loggerFactory.CreateLogger<UserService>();
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _setRepository = setRepository;
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

        user.Password = changes.Password ?? user.Password;
        user.Role = changes.Role ?? user.Role;

        user.IsLoggedIn = changes.IsLoggedIn ?? user.IsLoggedIn;

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

    public async Task<ICollection<Group>> GetUserGroups(string userId)
    {
        //this has to be a query where all the groups that a user is a member of are retrieved (might have to rewrite this as another linq query in the userRepository if it doesn't work)
        return await _groupRepository.GetAllIncludingAsync(g => g.Users.Where(u => u.Id == userId)).ToArrayAsync() ?? throw new NotFoundException("groups the user is a part of");
    }

    public async Task<ICollection<Set>> GetUserSets(string userId)
    {
        // same goes here since I now added a collection of users to a set (might also have to add a collection of sets to a user?) in order to grab the sets of a user...
        return await _setRepository.GetAllIncludingAsync(s => s.Users.Where(u => u.Id == userId)).ToArrayAsync() ?? throw new NotFoundException("sets of the user");
    }
}
