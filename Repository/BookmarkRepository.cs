using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class BookmarkRepository : Repository<Bookmark>, IBookmarkRepository
{
    public BookmarkRepository(DataContext context) : base(context, context.Bookmarks)
    {
    }
}
