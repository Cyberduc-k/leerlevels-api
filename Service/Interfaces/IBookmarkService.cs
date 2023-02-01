using Model;

namespace Service.Interfaces;

public interface IBookmarkService
{
    /**
     * <summary>
     * Get all bookmarks of the given user.<br/>
     * This includes two lists, one for the bookmarked targets and one for the bookmarked questions.
     * </summary>
     */
    public Task<(ICollection<Target>, ICollection<Mcq>)> GetBookmarksAsync(User user);

    /**
     * <summary>
     * Add a new bookmark to the given user.
     * </summary>
     */
    public Task<bool> AddBookmark(User user, Bookmark bookmark);

    /**
     * <summary>
     * Remove a bookmark from the given user.
     * </summary>
     */
    public Task DeleteBookmark(User user, string itemId, Bookmark.BookmarkType type);

    /**
     * <summary>
     * Check if the given user has a bookmark.
     * </summary>
     */
    public Task<bool> IsBookmarked(string userId, string itemId, Bookmark.BookmarkType type);
}
