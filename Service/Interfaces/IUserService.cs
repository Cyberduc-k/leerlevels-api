using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface IUserService
{
    /**
     * <summary>
     * Get all users.
     * </summary>
     * <param name="limit">limits the number of users returned</param>
     * <param name="page">specifies how many pages of <paramref name="limit"/> should be skipped</param>
     */
    public Task<ICollection<User>> GetUsers(int limit, int page);

    /**
     * <summary>
     * Get all users filtered by <see cref="User.UserName"/>
     * </summary>
     * <param name="limit">limits the number of users returned</param>
     * <param name="page">specifies how many pages of <paramref name="limit"/> should be skipped</param>
     * <param name="filter">the filter text to be used</param>
     */
    public Task<ICollection<User>> GetUsersFiltered(int limit, int page, string filter);

    /**
     * <summary>
     * Get a single user by its id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    Task<User> GetUserById(string userId);

    /**
     * <summary>
     * Create a new user.
     * </summary>
     */
    Task<User> CreateUser(User user);

    /**
     * <summary>
     * Update any property of the user with the given id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    Task<User> UpdateUser(string userId, UpdateUserDTO userDTO);

    /**
     * <summary>
     * Set the <see cref="User.IsActive"/> property of the user with the given id to false.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    Task DeleteUser(string userId);

    /**
     * <summary>
     * Get all groups the usr with the given id is in.
     * </summary>
     */
    Task<ICollection<Group>> GetUserGroups(string userId);

    /**
     * <summary>
     * Get all sets of the user with the given id.
     * </summary>
     */
    Task<ICollection<Set>> GetUserSets(string userId);
}
