using System.Linq.Expressions;
using Model;
using Xunit;

namespace Repository.Test;

public class SetRepositoryTests : RepositoryTestsBase<SetRepository, Set, string>
{
    public SetRepositoryTests()
    {
        _repository = new(_context);
    }

    protected override Set CreateMockEntity()
    {
        return new() {
            Id = Guid.NewGuid().ToString(),
        };
    }

    protected override Expression<Func<Set, bool>> CreateAnyTrueExpr(Set entity) => e => e.Id == entity.Id;
    protected override Expression<Func<Set, bool>> CreateAnyFalseExpr() => e => e.Id == "INVALID";

    [Fact]
    public async Task Get_All_Including_Targets_Async_Should_Return_Groups_With_Targets()
    {
        await _context.AddRangeAsync(
            CreateMockEntity(),
            CreateMockEntity(),
            CreateMockEntity()
        );
        await _context.SaveChangesAsync();

        IAsyncEnumerable<Set> entities = _repository.Include(s => s.Targets).GetAllAsync();

        Assert.Equal(3, await entities.CountAsync());
    }
}
