using AutoMapper;
using Model;
using Model.Response;
using Service.Interfaces;

namespace API.Mappings;

public class BookmarkedTargetConverter : ITypeConverter<(Target, string), Task<TargetResponse>>
{
    private readonly IBookmarkService _bookmarkService;

    public BookmarkedTargetConverter(IBookmarkService bookmarkService)
    {
        _bookmarkService = bookmarkService;
    }

    public async Task<TargetResponse> Convert((Target, string) source, Task<TargetResponse> destination, ResolutionContext context)
    {
        McqResponse[] mcqs = await source.Item1.Mcqs
            .ToAsyncEnumerable()
            .SelectAwait(async m => await context.Mapper.Map<Task<McqResponse>>((m, source.Item2)))
            .ToArrayAsync();

        return new() {
            IsBookmarked = await _bookmarkService.IsBookmarked(source.Item2, source.Item1.Id, Bookmark.BookmarkType.Target),
            Id = source.Item1.Id,
            TargetExplanation = source.Item1.TargetExplanation,
            Description = source.Item1.Description,
            ImageUrl = source.Item1.ImageUrl,
            Label = source.Item1.Label,
            YoutubeId = source.Item1.YoutubeId,
            Mcqs = mcqs,
        };

    }
}
