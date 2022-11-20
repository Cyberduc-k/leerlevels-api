using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Model;
using Moq;
using Repository.Interfaces;
using Service.Exceptions;
using Xunit;

namespace Service.Test;
public class GroupServiceTests
{
    private readonly Mock<IGroupRepository> _groupRepository;
    private readonly GroupService _service;

    public GroupServiceTests()
    {
        _groupRepository = new();
        _service = new GroupService(new LoggerFactory(), _groupRepository.Object);
    }

    [Fact]
    public async Task Get_All_Groups_Should_return_an_array_of_Groups()
    {
        Group[] mockGroups = new[] {
            new Group("1", "bestgroup", "this is our group", EducationType.Mavo, SchoolYear.One, null!, null!),
            new Group("2", "second bestgroup", "this is the rest of the groups", EducationType.Havo, SchoolYear.One, null!, null!),
        };

        _groupRepository.Setup(r => r.GetAllAsync()).Returns(mockGroups.ToAsyncEnumerable());

        ICollection<Group> groups = await _service.GetAllGroupsAsync();

        Assert.Equal(2, groups.Count);
    }

    [Fact]
    public async Task Get_Group_By_Id_Should_return_A_Group_Object()
    {
        string groupId = "1";
        _groupRepository.Setup(r => r.Include(g => g.Sets).Include(g => g.Users).GetByAsync(g => g.Id == groupId)).ReturnsAsync(() => new Group("1",
           "second bestgroup",
           "this is the rest of the groups",
           EducationType.Havo,
           SchoolYear.One,
           null!,
           null!));
        Group group = await _service.GetGroupByIdAsync(groupId);

        Assert.Equal("1", group.Id);
    }

    [Fact]
    public void Get_Group_By_Id_should_Throws_Not_Found_Exception()
    {
        _groupRepository.Setup(r => r.Include(g => g.Sets).Include(g => g.Users).GetByAsync(It.IsAny<Expression<Func<Group, bool>>>())).ReturnsAsync(() => null);
        Assert.ThrowsAsync<NotFoundException>(() => _service.GetGroupByIdAsync("3"));
    }

    [Fact]
    public void Get_Group_By_Id_should_Throws_null_Exception()
    {
        _groupRepository.Setup(r => r.Include(g => g.Sets).Include(g => g.Users).GetByAsync(It.IsAny<Expression<Func<Group, bool>>>())).ReturnsAsync(() => null);
        Assert.ThrowsAsync<NullReferenceException>(() => _service.GetGroupByIdAsync(""));
    }
}
