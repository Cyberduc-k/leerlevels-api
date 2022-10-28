using System.Linq.Expressions;
using Model;
using Xunit;

namespace Repository.Test;

public class GroupRepositoryTests : RepositoryTestsBase<GroupRepository, Group, string>
{
    public GroupRepositoryTests()
    {
        _repository = new(_context);
    }

    protected override Group CreateMockEntity()
    {
        return new() {
            Id = Guid.NewGuid().ToString(),
            Name = "Name",
            Subject = "Subject",
            EducationType = EducationType.Mavo,
            SchoolYear = SchoolYear.Seven,
        };
    }

    protected override Expression<Func<Group, bool>> CreateAnyTrueExpr(Group entity) => e => e.Name == entity.Name;
    protected override Expression<Func<Group, bool>> CreateAnyFalseExpr() => e => e.Name == "INVALID";

    [Fact]
    public async Task Get_All_Including_Sets_And_Users_Async_Should_Return_Groups_With_Sets_And_Users()
    {
        await _context.AddRangeAsync(
            CreateMockEntity(),
            CreateMockEntity(),
            CreateMockEntity()
        );
        await _context.SaveChangesAsync();

        IAsyncEnumerable<Group> entities = _repository.Include(g => g.Sets).Include(g => g.Users).GetAllAsync();

        Assert.Equal(3, await entities.CountAsync());
    }
}
