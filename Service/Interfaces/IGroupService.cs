using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Service.Interfaces;
public interface IGroupService
{
    Task<ICollection<Group>> GetAllGroupsAsync();

    Task<Group> GetGroupByIdAsync(string groupId);

    public Task<bool> IsGroupExistsAsync(string id);
}
