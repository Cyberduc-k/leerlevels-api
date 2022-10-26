using System.Linq.Expressions;
using Model;

namespace Repository.Test;

public class BookmarkRepositoryTests : RepositoryTestsBase<BookmarkRepository, Bookmark, (string UserId, string ItemId, Bookmark.BookmarkType Type)>
{
    public BookmarkRepositoryTests() : base(e => (e.UserId, e.ItemId, e.Type))
    {
        _repository = new(_context);
    }

    protected override Bookmark CreateMockEntity()
    {
        return new() {
            Type = Bookmark.BookmarkType.Target,
            UserId = Guid.NewGuid().ToString(),
            ItemId = Guid.NewGuid().ToString(),
        };
    }

    protected override Expression<Func<Bookmark, object>> CreateIncludeExpr() => null!;
    protected override Expression<Func<Bookmark, bool>> CreateAnyTrueExpr(Bookmark entity) => e => e.Type == entity.Type;
    protected override Expression<Func<Bookmark, bool>> CreateAnyFalseExpr() => e => e.Type == Bookmark.BookmarkType.Mcq;
}
