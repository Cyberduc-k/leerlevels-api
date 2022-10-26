using Model;

namespace Repository.Interfaces;

public interface IBookmarkRepository : IRepository<Bookmark, (string UserId, string ItemId, Bookmark.BookmarkType Type)>
{
}
