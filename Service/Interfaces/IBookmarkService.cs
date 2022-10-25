using Model;

namespace Service.Interfaces;

public interface IBookmarkService
{
    public Task<(ICollection<Target>, ICollection<Mcq>)> GetBookmarksAsync(User user);
    public Task AddBookmark(User user, Bookmark bookmark);
}
