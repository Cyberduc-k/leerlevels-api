using System.Linq.Expressions;
using Model;

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

    protected override Expression<Func<Mcq, object>> CreateIncludeExpr() => e => e.AnswerOptions;
    protected override Expression<Func<Mcq, bool>> CreateAnyTrueExpr(Mcq entity) => e => e.QuestionText == entity.QuestionText;
    protected override Expression<Func<Mcq, bool>> CreateAnyFalseExpr() => e => e.QuestionText == "INVALID";
}
