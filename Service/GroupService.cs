using Microsoft.Extensions.Logging;
using Model;
using Repository.Interfaces;
using Service.Exceptions;
using Service.Interfaces;

namespace Service;

public class GroupService : IGroupService
{
    private readonly ILogger _logger;
    private readonly IGroupRepository _groupRepsitory;

    public GroupService(ILoggerFactory loggerFactory, IGroupRepository groupRepository)
    {
        _logger = loggerFactory.CreateLogger<GroupService>();
        _groupRepsitory = groupRepository;
    }

    public async Task<ICollection<Group>> GetAllGroupsAsync()
    {
        return await _groupRepsitory.GetAllAsync().ToArrayAsync();
    }

    public async Task<Group> GetGroupByIdAsync(string groupId)
    {
        return await _groupRepsitory.GetByIdAsync(groupId) ?? throw new NotFoundException("group");
    }
}
