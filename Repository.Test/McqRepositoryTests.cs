using System.Linq.Expressions;
using Model;
using Xunit;

namespace Repository.Test;

public class McqRepositoryTests : RepositoryTestsBase<McqRepository, Mcq, string>
{
    public McqRepositoryTests()
    {
        _repository = new(_context);
    }

    protected override Mcq CreateMockEntity()
    {
        return new() {
            Id = Guid.NewGuid().ToString(),
            QuestionText = "QuestionText",
            Explanation = "Explanation",
            AllowRandom = true,
        };
    }

    protected override Expression<Func<Mcq, bool>> CreateAnyTrueExpr(Mcq entity) => e => e.QuestionText == entity.QuestionText;
    protected override Expression<Func<Mcq, bool>> CreateAnyFalseExpr() => e => e.QuestionText == "INVALID";

    [Fact]
    public async Task Get_All_Including_Answer_Options_Async_Should_Return_Groups_With_Answer_Options()
    {
        await _context.AddRangeAsync(
            CreateMockEntity(),
            CreateMockEntity(),
            CreateMockEntity()
        );
        await _context.SaveChangesAsync();

        IAsyncEnumerable<Mcq> entities = _repository.Include(m => m.AnswerOptions).GetAllAsync();

        Assert.Equal(3, await entities.CountAsync());
    }
}
