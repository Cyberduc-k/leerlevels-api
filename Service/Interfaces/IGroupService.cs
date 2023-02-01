using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface IGroupService
{
    /**
     * <summary>
     * Get all groups.
     * </summary>
     */
    Task<ICollection<Group>> GetAllGroupsAsync();

    /**
     * <summary>
     * Get a single group by its id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    Task<Group> GetGroupByIdAsync(string groupId);

    /**
     * <summary>
     * Add a user to the group with the given id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    Task<Group> AddGrouptoUser(string id, string userid);

    /**
     * <summary>
     * Check if the group with the given id exists.
     * </summary>
     */
    public Task<bool> GroupExistsAsync(string id);

    /**
     * <summary>
     * Create a new group.
     * </summary>
     */
    public Task<Group> CreateGroup(Group newGroup);

    /**
     * <summary>
     * Update the <see cref="Group.Name"/>, <see cref="Group.Subject"/>, <see cref="Group.EducationType"/> or <see cref="Group.SchoolYear"/> of the group with the given id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    public Task UpdateGroup(string groupId, UpdateGroupDTO changes);

    /**
     * <summary>
     * Delete the group with the given id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    public Task DeleteGroup(string groupId);
}
