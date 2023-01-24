using Model;

namespace Service.Interfaces;

public interface IBookmarkService
{
    public Task<(ICollection<Target>, ICollection<Mcq>)> GetBookmarksAsync(User user);
    public Task<bool> AddBookmark(User user, Bookmark bookmark);
    public Task DeleteBookmark(User user, string itemId, Bookmark.BookmarkType type);
    public Task<bool> IsBookmarked(string userId, string itemId, Bookmark.BookmarkType type);
}
