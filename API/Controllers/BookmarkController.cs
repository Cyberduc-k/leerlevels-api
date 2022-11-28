using System.Net;
using API.Attributes;
using API.Examples;
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
    private readonly IUserService _userService;

    public BookmarkController(ILoggerFactory loggerFactory, ITokenService tokenService, IMapper mapper, IBookmarkService bookmarkService, IUserService userService)
        : base(loggerFactory.CreateLogger<BookmarkController>(), tokenService)
    {
        _mapper = mapper;
        _bookmarkService = bookmarkService;
        _userService = userService;
    }

    [Function(nameof(GetBookmarks))]
    [OpenApiOperation(operationId: nameof(GetBookmarks), tags: new[] { "Bookmarks" }, Summary = "A list of bookmarks", Description = "Returns a list of all bookmarks for the current user")]
    [OpenApiAuthentication]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(BookmarksResponse), Description = "A list of bookmarks", Example = typeof(BookmarksResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetBookmarks(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "bookmarks")] HttpRequestData req)
    {
        string userId = await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/bookmarks");
        (ICollection<Target> targets, ICollection<Mcq> mcqs) = await _bookmarkService.GetBookmarksAsync(await _userService.GetUserById(userId));
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
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(BookmarkDTO), Required = true, Description = "The new bookmark", Example = typeof(BookmarkDTOExample))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK)]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.Created)]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> AddBookmark(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "bookmarks")] HttpRequestData req)
    {
        string userId = await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/bookmarks");
        BookmarkDTO? bookmarkDTO = await req.ReadFromJsonAsync<BookmarkDTO>();
        Bookmark bookmark = new(bookmarkDTO!.ItemId, bookmarkDTO.Type);
        bool added = await _bookmarkService.AddBookmark(await _userService.GetUserById(userId), bookmark);

        return req.CreateResponse(added ? HttpStatusCode.Created : HttpStatusCode.OK);
    }

    [Function(nameof(DeleteBookmark))]
    [OpenApiOperation(operationId: nameof(DeleteBookmark), tags: new[] { "Bookmarks" }, Summary = "Delete a bookmark", Description = "Delete a bookmark for the current user")]
    [OpenApiAuthentication]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(BookmarkDTO), Required = true, Description = "The bookmark to delete", Example = typeof(BookmarkDTOExample))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK)]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the bookmark.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> DeleteBookmark(
        [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "bookmarks")] HttpRequestData req)
    {
        string userId = await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/bookmarks");
        BookmarkDTO? bookmarkDTO = await req.ReadFromJsonAsync<BookmarkDTO>();

        await _bookmarkService.DeleteBookmark(await _userService.GetUserById(userId), bookmarkDTO!.ItemId, bookmarkDTO.Type);

        return req.CreateResponse(HttpStatusCode.OK);
    }
}
