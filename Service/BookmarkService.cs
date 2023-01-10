using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Interfaces;
using Service.Exceptions;
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
        IAsyncEnumerable<Bookmark> bookmarks = _bookmarkRepository.Where(b => b.UserId == user.Id).GetAllAsync();
        Target[] targets = await bookmarks
            .Where(b => b.Type == Bookmark.BookmarkType.Target)
            .SelectAwait(async b => await _targetRepository.GetByIdAsync(b.ItemId))
            .Where(t => t is not null)
            .Select(t => t!)
            .ToArrayAsync();

        Mcq[] mcqs = await bookmarks
            .Where(b => b.Type == Bookmark.BookmarkType.Mcq)
            .SelectAwait(async b => await _mcqRepository.GetByIdAsync(b.ItemId)!)
            .Where(m => m is not null)
            .Select(m => m!)
            .ToArrayAsync();

        return (targets, mcqs);
    }

    public async Task<bool> AddBookmark(User user, Bookmark bookmark)
    {
        bookmark.UserId = user.Id;

        if (!await _bookmarkRepository.AnyAsync(b => b == bookmark)) {
            await _bookmarkRepository.InsertAsync(bookmark);
            await _bookmarkRepository.SaveChanges();
            return true;
        }

        return false;
    }

    public async Task DeleteBookmark(User user, string itemId, Bookmark.BookmarkType type)
    {
        Bookmark bookmark = await _bookmarkRepository.GetByIdAsync((user.Id, itemId, type)) ?? throw new NotFoundException("bookmark");
        _bookmarkRepository.Remove(bookmark);
        await _bookmarkRepository.SaveChanges();
    }

    public async Task<bool> IsBookmarked(string itemId, Bookmark.BookmarkType type)
    {
        return await _bookmarkRepository.AnyAsync(x => x.ItemId == itemId && x.Type == type);
    }
}