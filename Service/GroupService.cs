using Microsoft.EntityFrameworkCore;
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
    private readonly IUserRepository _userRepository;

    public GroupService(ILoggerFactory loggerFactory, IGroupRepository groupRepository)
    {
        _logger = loggerFactory.CreateLogger<GroupService>();
        _groupRepsitory = groupRepository;
    }

    public async Task<Group> AddGrouptoUser(string id, string userId)
    {
        Group group = await _groupRepsitory.GetByIdAsync(id) ?? throw new NotFoundException("group");
        User user = await _userRepository.Include(u => u.Groups).GetByAsync(u => u.Id == userId) ?? throw new NotFoundException("user");

        user.Groups.Add(group);
        await _userRepository.SaveChanges();
        return group;
    }

    public async Task<ICollection<Group>> GetAllGroupsAsync()
    {
        return await _groupRepsitory
            .Include(g => g.Users)
            .Include(g => g.Sets)
            .ThenInclude(s => s.Targets)
            .ThenInclude(t => t.Mcqs)
            .ThenInclude(m => m.AnswerOptions)
            .GetAllAsync()
            .ToArrayAsync();
    }

    public async Task<Group> GetGroupByIdAsync(string groupId)
    {
        return await _groupRepsitory
            .Include(g => g.Sets)
            .ThenInclude(s => s.Targets)
            .ThenInclude(t => t.Mcqs)
            .ThenInclude(m => m.AnswerOptions)
            .Include(g => g.Users)
            .GetByAsync(g => g.Id == groupId) ?? throw new NotFoundException("group");
    }

    public async Task<bool> GroupExistsAsync(string id)
    {
        return await _groupRepsitory.AnyAsync(x => x.Id == id);
    }
}
