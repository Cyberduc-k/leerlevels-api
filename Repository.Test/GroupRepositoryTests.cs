using System.Linq.Expressions;
using Model;

namespace Repository.Test;

public class GroupRepositoryTests : RepositoryTestsBase<GroupRepository, Group, string>
{
    public GroupRepositoryTests()
    {
        _repository = new(_context);
    }

    protected override Group CreateMockEntity()
    {
        return new() {
            Id = Guid.NewGuid().ToString(),
            Name = "Name",
            Subject = "Subject",
            EducationType = EducationType.Mavo,
            SchoolYear = SchoolYear.Seven,
        };
    }

    protected override Expression<Func<Group, object>> CreateIncludeExpr() => e => e.Set;
    protected override Expression<Func<Group, bool>> CreateAnyTrueExpr(Group entity) => e => e.Name == entity.Name;
    protected override Expression<Func<Group, bool>> CreateAnyFalseExpr() => e => e.Name == "INVALID";
}
