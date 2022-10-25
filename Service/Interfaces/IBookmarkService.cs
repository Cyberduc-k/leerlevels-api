using Model;
using Model.Response;

namespace Service.Interfaces;

public interface IBookmarkService
{
    public Task<BookmarksResponse> GetBookmarksAsync(User user);
    public Task AddBookmark(User user, Bookmark bookmark);
}
