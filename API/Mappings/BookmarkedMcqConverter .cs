using AutoMapper;
using Model;
using Model.Response;
using Service.Interfaces;

namespace API.Mappings;

public class BookmarkedMcqConverter : ITypeConverter<(Mcq, string), Task<McqResponse>>
{
    private readonly IBookmarkService _bookmarkService;

    public BookmarkedMcqConverter(IBookmarkService bookmarkService)
    {
        _bookmarkService = bookmarkService;
    }

    public async Task<McqResponse> Convert((Mcq, string) source, Task<McqResponse> destination, ResolutionContext context)
    {
        return new McqResponse {
            IsBookmarked = await _bookmarkService.IsBookmarked(source.Item2, source.Item1.Id, Bookmark.BookmarkType.Mcq),
            Id = source.Item1.Id,
            TargetId = source.Item1.Target?.Id!,
            AllowRandom = source.Item1.AllowRandom,
            AnswerOptions = source.Item1.AnswerOptions,
            Explanation = source.Item1.Explanation,
            QuestionText = source.Item1.QuestionText,
        };
    }
}
