using Model;

namespace Service.Interfaces;
public interface IGroupService
{
    Task<ICollection<Group>> GetAllGroupsAsync();

    Task<Group> GetGroupByIdAsync(string groupId);

    Task<Group> CreateGroup(Group group);
    public Task<bool> GroupExistsAsync(string id);
}
