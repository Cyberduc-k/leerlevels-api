using AutoMapper;
using Model;
using Model.Response;
using Service.Interfaces;

namespace API.Mappings;

public class BookmarkedMcqConverter : ITypeConverter<Mcq, Task<McqResponse>>
{
    private readonly IBookmarkService _bookmarkService;

    public BookmarkedMcqConverter(IBookmarkService bookmarkService)
    {
        _bookmarkService = bookmarkService;
    }

    public async Task<McqResponse> Convert(Mcq source, Task<McqResponse> destination, ResolutionContext context)
    {
        return new McqResponse {
            IsBookedmarked = await _bookmarkService.IsBookmarked(source.Id, Bookmark.BookmarkType.Mcq),
            Id = source.Id,
            TargetId = source.Target?.Id!,
            AllowRandom = source.AllowRandom,
            AnswerOptions = source.AnswerOptions,
            Explanation = source.Explanation,
            QuestionText = source.QuestionText,
        };
    }
}
