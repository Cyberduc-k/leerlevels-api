using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Model;
using Repository;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service;
public class GroupService : IGroupService
{
    private readonly IGroupRepository groupRepository;
    private ILogger _Logger { get; }
    public GroupService(IGroupRepository groupRepository, ILoggerFactory logger)
    {
        this.groupRepository = groupRepository;
        _Logger = logger.CreateLogger<GroupService>();
    }

    public async Task<ICollection<Group>> GetAllGroupsAsync()
    {
        return await groupRepository.GetAllAsync().ToArrayAsync();
    }

    public async Task<Group> GetGroupByIdAsync(string groupId)
    {
        return await groupRepository.GetByIdAsync(groupId) ?? throw new NullReferenceException();
    }
}
