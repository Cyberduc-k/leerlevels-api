using System.Linq.Expressions;
using Model;

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

    protected override Expression<Func<Set, object>> CreateIncludeExpr() => e => e.Targets;
    protected override Expression<Func<Set, bool>> CreateAnyTrueExpr(Set entity) => e => e.Id == entity.Id;
    protected override Expression<Func<Set, bool>> CreateAnyFalseExpr() => e => e.Id == "INVALID";
}
