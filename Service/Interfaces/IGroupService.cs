using Model;

namespace Service.Interfaces;
public interface IGroupService
{
    Task<ICollection<Group>> GetAllGroupsAsync();

    Task<Group> GetGroupByIdAsync(string groupId);

    public Task<bool> GroupExistsAsync(string id);
}
