using System.Linq.Expressions;
using Model;

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

    protected override Expression<Func<Forum, object>> CreateIncludeExpr() => null!;
    protected override Expression<Func<Forum, bool>> CreateAnyTrueExpr(Forum entity) => e => e.Title == entity.Title;
    protected override Expression<Func<Forum, bool>> CreateAnyFalseExpr() => e => e.Title == "INVALID";
}
