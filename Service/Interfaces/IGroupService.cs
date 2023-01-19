using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface IGroupService
{
    Task<ICollection<Group>> GetAllGroupsAsync();

    Task<Group> GetGroupByIdAsync(string groupId);

    Task<Group> AddGrouptoUser(string id, string userid);
    public Task<bool> GroupExistsAsync(string id);

    public Task<Group> CreateGroup(Group newGroup);
    public Task UpdateGroup(string groupId, UpdateGroupDTO changes);
    public Task DeleteGroup(string groupId);
}
