using System.Linq.Expressions;
using Model;
using Xunit;

namespace Repository.Test;

public class TargetRepositoryTests : RepositoryTestsBase<TargetRepository, Target, string>
{
    public TargetRepositoryTests()
    {
        _repository = new(_context);
    }

    protected override Target CreateMockEntity()
    {
        return new() {
            Id = Guid.NewGuid().ToString(),
            Label = "Label",
            Description = "Description",
            TargetExplanation = "TargetExplanation",
            YoutubeId = "YoutubeId",
            ImageUrl = "ImageUrl",
        };
    }

    protected override Expression<Func<Target, bool>> CreateAnyTrueExpr(Target entity) => e => e.Label == entity.Label;
    protected override Expression<Func<Target, bool>> CreateAnyFalseExpr() => e => e.Label == "INVALID";

    [Fact]
    public async Task Get_All_Including_Mcqs_Async_Should_Return_Groups_With_Mcqs()
    {
        await _context.AddRangeAsync(
            CreateMockEntity(),
            CreateMockEntity(),
            CreateMockEntity()
        );
        await _context.SaveChangesAsync();

        IAsyncEnumerable<Target> entities = _repository.Include(t => t.Mcqs).GetAllAsync();

        Assert.Equal(3, await entities.CountAsync());
    }
}
