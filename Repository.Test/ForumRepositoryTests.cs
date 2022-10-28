using System.Linq.Expressions;
using Model;
using Xunit;

namespace Repository.Test;

public class ForumRepositoryTests : RepositoryTestsBase<ForumRepository, Forum, string>
{
    public ForumRepositoryTests()
    {
        _repository = new(_context);
    }

    protected override Forum CreateMockEntity()
    {
        return new() {
            Id = Guid.NewGuid().ToString(),
            Title = "Title",
            Description = "description"
        };
    }

    protected override Expression<Func<Forum, bool>> CreateAnyTrueExpr(Forum entity) => e => e.Title == entity.Title;
    protected override Expression<Func<Forum, bool>> CreateAnyFalseExpr() => e => e.Title == "INVALID";

    [Fact]
    public async Task Get_All_Including_Replies_Async_Should_Return_Forums_With_Replies()
    {
        await _context.AddRangeAsync(
            CreateMockEntity(),
            CreateMockEntity(),
            CreateMockEntity()
        );
        await _context.SaveChangesAsync();

        IAsyncEnumerable<Forum> entities = _repository.Include(f => f.Replies).GetAllAsync();

        Assert.Equal(3, await entities.CountAsync());
    }
}
