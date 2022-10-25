using Model;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service;

public class BookmarkService : IBookmarkService
{
    private readonly IBookmarkRepository _bookmarkRepository;
    private readonly ITargetRepository _targetRepository;
    private readonly IMcqRepository _mcqRepository;

    public BookmarkService(IBookmarkRepository bookmarkRepository, ITargetRepository targetRepository, IMcqRepository mcqRepository)
    {
        _bookmarkRepository = bookmarkRepository;
        _targetRepository = targetRepository;
        _mcqRepository = mcqRepository;
    }

    public async Task<(ICollection<Target>, ICollection<Mcq>)> GetBookmarksAsync(User user)
    {
        IAsyncEnumerable<Bookmark> bookmarks = _bookmarkRepository.GetAllAsync().Where(b => b.UserId == user.Id);
        Target[] targets = await bookmarks
            .Where(b => b.Type == Bookmark.BookmarkType.Target)
            .SelectAwait(async b => await _targetRepository.GetByIdAsync(b.ItemId))
            .Select(t => t!)
            .ToArrayAsync();

        Mcq[] mcqs = await bookmarks
            .Where(b => b.Type == Bookmark.BookmarkType.Mcq)
            .SelectAwait(async b => await _mcqRepository.GetByIdAsync(b.ItemId)!)
            .Select(m => m!)
            .ToArrayAsync();

        return (targets, mcqs);
    }

    public async Task AddBookmark(User user, Bookmark bookmark)
    {
        bookmark.UserId = user.Id;
        await _bookmarkRepository.InsertAsync(bookmark);
        await _bookmarkRepository.SaveChanges();
    }
}
