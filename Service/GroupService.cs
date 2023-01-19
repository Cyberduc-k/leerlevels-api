using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Repository.Interfaces;
using Service.Exceptions;
using Service.Interfaces;

namespace Service;

public class GroupService : IGroupService
{
    private readonly ILogger _logger;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;

    public GroupService(ILoggerFactory loggerFactory, IGroupRepository groupRepository, IUserRepository userRepository)
    {
        _logger = loggerFactory.CreateLogger<GroupService>();
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }

    public async Task<Group> AddGrouptoUser(string id, string userId)
    {
        Group group = await _groupRepository.GetByIdAsync(id) ?? throw new NotFoundException("group");
        User user = await _userRepository.Include(u => u.Groups).GetByAsync(u => u.Id == userId) ?? throw new NotFoundException("user");

        user.Groups.Add(group);
        await _userRepository.SaveChanges();
        return group;
    }

    public async Task<ICollection<Group>> GetAllGroupsAsync()
    {
        return await _groupRepository
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
        return await _groupRepository
            .Include(g => g.Sets)
            .ThenInclude(s => s.Targets)
            .ThenInclude(t => t.Mcqs)
            .ThenInclude(m => m.AnswerOptions)
            .Include(g => g.Users)
            .GetByAsync(g => g.Id == groupId) ?? throw new NotFoundException("group");
    }

    public async Task<bool> GroupExistsAsync(string id)
    {
        return await _groupRepository.AnyAsync(x => x.Id == id);
    }

    public async Task<Group> CreateGroup(Group newGroup)
    {
        newGroup.Id = Guid.NewGuid().ToString();
        newGroup.Sets = new List<Set>();
        newGroup.Users = new List<User>();

        await _groupRepository.InsertAsync(newGroup);
        await _groupRepository.SaveChanges();
        return newGroup;
    }

    public async Task UpdateGroup(string groupId, UpdateGroupDTO changes)
    {
        Group group = await GetGroupByIdAsync(groupId);
        group.Name = changes.Name ?? group.Name;
        group.Subject = changes.Subject ?? group.Subject;
        group.EducationType = changes.EducationType ?? group.EducationType;
        group.SchoolYear = changes.SchoolYear ?? group.SchoolYear;
        await _groupRepository.SaveChanges();
    }

    public async Task DeleteGroup(string GroupId)
    {
        Group Group = await GetGroupByIdAsync(GroupId);

        _groupRepository.Remove(Group);
        await _groupRepository.SaveChanges();
    }
}
