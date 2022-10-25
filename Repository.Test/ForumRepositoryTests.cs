using Model;
using Xunit;

namespace Repository.Test;

public class ForumRepositoryTests : RepositoryTestsBase<ForumRepository, Forum>
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

    [Fact]
    public async Task Any_Async_Should_Return_True()
    {
        Forum mockEntity = CreateMockEntity();
        await _context.AddAsync(mockEntity);
        await _context.SaveChangesAsync();

        bool any = await _repository.AnyAsync(e => e.Title == mockEntity.Title);

        Assert.True(any);
    }

    [Fact]
    public async Task Any_Async_Should_Return_False()
    {
        bool any = await _repository.AnyAsync(e => e.Title == "INVALID");

        Assert.False(any);
    }
}
