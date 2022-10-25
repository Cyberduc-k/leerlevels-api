using AutoMapper;
using Model;
using Model.Response;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service;

public class BookmarkService : IBookmarkService
{
    private readonly IMapper _mapper;
    private readonly IBookmarkRepository _bookmarkRepository;
    private readonly ITargetRepository _targetRepository;
    private readonly IMcqRepository _mcqRepository;

    public BookmarkService(IMapper mapper, IBookmarkRepository bookmarkRepository, ITargetRepository targetRepository, IMcqRepository mcqRepository)
    {
        _mapper = mapper;
        _bookmarkRepository = bookmarkRepository;
        _targetRepository = targetRepository;
        _mcqRepository = mcqRepository;
    }

    public async Task<BookmarksResponse> GetBookmarksAsync(User user)
    {
        IAsyncEnumerable<Bookmark> bookmarks = _bookmarkRepository.GetAllAsync().Where(b => b.UserId == user.Id);
        TargetResponse[] targets = await bookmarks
            .Where(b => b.Type == Bookmark.BookmarkType.Target)
            .SelectAwait(async b => await _targetRepository.GetByIdAsync(b.ItemId))
            .Select(t => _mapper.Map<TargetResponse>(t!))
            .ToArrayAsync();

        McqResponse[] mcqs = await bookmarks
            .Where(b => b.Type == Bookmark.BookmarkType.Mcq)
            .SelectAwait(async b => await _mcqRepository.GetByIdAsync(b.ItemId)!)
            .Select(m => _mapper.Map<McqResponse>(m!))
            .ToArrayAsync();

        return new(targets, mcqs);
    }

    public async Task AddBookmark(User user, Bookmark bookmark)
    {
        bookmark.UserId = user.Id;
        await _bookmarkRepository.InsertAsync(bookmark);
        await _bookmarkRepository.SaveChanges();
    }
}
