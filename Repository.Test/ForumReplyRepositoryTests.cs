using Model;
using Xunit;

namespace Repository.Test;

public class ForumReplyRepositoryTests : RepositoryTestsBase<ForumReplyRepository, ForumReply>
{
    public ForumReplyRepositoryTests()
    {
        _repository = new(_context);
    }

    protected override ForumReply CreateMockEntity()
    {
        return new() {
            Id = Guid.NewGuid().ToString(),
            Text = "Text",
        };
    }

    [Fact]
    public async Task Any_Async_Should_Return_True()
    {
        ForumReply mockEntity = CreateMockEntity();
        await _context.AddAsync(mockEntity);
        await _context.SaveChangesAsync();

        bool any = await _repository.AnyAsync(e => e.Text == mockEntity.Text);

        Assert.True(any);
    }

    [Fact]
    public async Task Any_Async_Should_Return_False()
    {
        bool any = await _repository.AnyAsync(e => e.Text == "INVALID");

        Assert.False(any);
    }
}
