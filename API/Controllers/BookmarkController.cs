using System.Net;
using API.Attributes;
using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Model.Response;
using Service.Interfaces;

namespace API.Controllers;

public class BookmarkController : ControllerWithAuthentication
{
    private readonly IMapper _mapper;
    private readonly IBookmarkService _bookmarkService;

    public BookmarkController(ILoggerFactory loggerFactory, ITokenService tokenService, IMapper mapper, IBookmarkService bookmarkService)
        : base(loggerFactory.CreateLogger<BookmarkController>(), tokenService)
    {
        _mapper = mapper;
        _bookmarkService = bookmarkService;
    }

    [Function(nameof(GetBookmarks))]
    [OpenApiOperation(operationId: nameof(GetBookmarks), tags: new[] { "Bookmarks" }, Summary = "A list of bookmarks", Description = "Returns a list of all bookmarks for the current user")]
    [OpenApiAuthentication]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(BookmarksResponse), Description = "A list of bookmarks")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError)]
    public async Task<HttpResponseData> GetBookmarks(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "bookmarks")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/bookmarks");
        (ICollection<Target> targets, ICollection<Mcq> mcqs) = await _bookmarkService.GetBookmarksAsync(_tokenService.User);
        BookmarksResponse bookmarks = new(
            targets.Select(t => _mapper.Map<TargetResponse>(t)).ToArray(),
            mcqs.Select(m => _mapper.Map<McqResponse>(m)).ToArray()
        );

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(bookmarks);

        return res;
    }

    [Function(nameof(AddBookmark))]
    [OpenApiOperation(operationId: nameof(AddBookmark), tags: new[] { "Bookmarks" }, Summary = "Add a bookmark", Description = "Add a new bookmark for the current user")]
    [OpenApiAuthentication]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(BookmarkDTO), Required = true, Description = "The new bookmark")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK)]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError)]
    public async Task<HttpResponseData> AddBookmark(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "bookmarks")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/bookmarks");
        BookmarkDTO? bookmarkDTO = await req.ReadFromJsonAsync<BookmarkDTO>();
        Bookmark bookmark = new(bookmarkDTO!.ItemId, bookmarkDTO.Type);

        await _bookmarkService.AddBookmark(_tokenService.User, bookmark);

        return req.CreateResponse(HttpStatusCode.OK);
    }
}
