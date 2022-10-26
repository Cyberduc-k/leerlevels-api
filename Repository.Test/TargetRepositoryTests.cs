using System.Linq.Expressions;
using Model;

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

    protected override Expression<Func<Target, object>> CreateIncludeExpr() => e => e.Mcqs;
    protected override Expression<Func<Target, bool>> CreateAnyTrueExpr(Target entity) => e => e.Label == entity.Label;
    protected override Expression<Func<Target, bool>> CreateAnyFalseExpr() => e => e.Label == "INVALID";
}
