using AutoMapper;
using Model;
using Model.Response;
using Service.Interfaces;

namespace API.Mappings;

public class BookmarkedTargetConverter : ITypeConverter<Target, Task<TargetResponse>>
{
    private readonly IBookmarkService _bookmarkService;

    public BookmarkedTargetConverter(IBookmarkService bookmarkService)
    {
        _bookmarkService = bookmarkService;
    }

    public async Task<TargetResponse> Convert(Target source, Task<TargetResponse> destination, ResolutionContext context)
    {
        McqResponse[] mcqs = await Task.WhenAll(context.Mapper.Map<Task<McqResponse>[]>(source.Mcqs));

        return new() {
            IsBookmarked = await _bookmarkService.IsBookmarked(source.Id, Bookmark.BookmarkType.Target),
            Id = source.Id,
            TargetExplanation = source.TargetExplanation,
            Description = source.Description,
            ImageUrl = source.ImageUrl,
            Label = source.Label,
            YoutubeId = source.YoutubeId,
            Mcqs = mcqs,
        };

    }
}
