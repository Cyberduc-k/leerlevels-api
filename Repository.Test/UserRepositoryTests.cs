using System.Linq.Expressions;
using Model;

namespace Repository.Test;

public class UserRepositoryTests : RepositoryTestsBase<UserRepository, User, string>
{
    public UserRepositoryTests()
    {
        _repository = new(_context);
    }

    protected override User CreateMockEntity()
    {
        return new() {
            Id = Guid.NewGuid().ToString(),
            Email = "Email",
            FirstName = "FirstName",
            LastName = "LastName",
            UserName = "UserName",
            Password = "Password",
            Role = UserRole.Student,
            LastLogin = DateTime.UtcNow,
            LastDeviceHandle = "LastDeviceHandle",
            ShareCode = "ShareCode",
            IsActive = true,
        };
    }

    protected override Expression<Func<User, bool>> CreateAnyTrueExpr(User entity) => e => e.UserName == entity.UserName;
    protected override Expression<Func<User, bool>> CreateAnyFalseExpr() => e => e.UserName == "INVALID";
}
