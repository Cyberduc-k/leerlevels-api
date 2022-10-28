using System.Linq.Expressions;
using Model;

namespace Repository.Test;

public class ForumReplyRepositoryTests : RepositoryTestsBase<ForumReplyRepository, ForumReply, string>
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

    protected override Expression<Func<ForumReply, bool>> CreateAnyTrueExpr(ForumReply entity) => e => e.Text == entity.Text;
    protected override Expression<Func<ForumReply, bool>> CreateAnyFalseExpr() => e => e.Text == "INVALID";
}
