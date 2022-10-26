using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class BookmarkRepository : Repository<Bookmark, (string UserId, string ItemId, Bookmark.BookmarkType Type)>, IBookmarkRepository
{
    public BookmarkRepository(DataContext context) : base(context, context.Bookmarks)
    {
    }

    public override async Task<Bookmark?> GetByIdAsync((string UserId, string ItemId, Bookmark.BookmarkType Type) id)
    {
        return await _dbset.FindAsync(id.UserId, id.ItemId, id.Type);
    }
}
