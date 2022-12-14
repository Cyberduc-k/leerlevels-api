using Model;

namespace Service.Interfaces;
public interface IGroupService
{
    Task<ICollection<Group>> GetAllGroupsAsync();

    Task<Group> GetGroupByIdAsync(string groupId);

    Task<Group> AddGrouptoUser(string id, string userid);
    public Task<bool> GroupExistsAsync(string id);
}
