using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Model.Response;
using Newtonsoft.Json;
using Service.Interfaces;

namespace API.Controllers;

public class ForumController
{
    private readonly ILogger _logger;
    private readonly IForumService _forumService;
    private readonly IUserService _userService;

    public ForumController(ILoggerFactory loggerFactory, IForumService forumService, IUserService userService)
    {
        _logger = loggerFactory.CreateLogger<ForumController>();
        _forumService = forumService;
        _userService = userService;
    }

    [Function(nameof(GetForums))]
    [OpenApiOperation(operationId: nameof(GetForums), tags: new[] { "Forums" }, Summary = "A list of forum posts", Description = "Returns a list of all forum posts.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ForumResponse[]), Description = "A list of forum posts.")]
    public async Task<HttpResponseData> GetForums([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "forums")] HttpRequestData req)
    {
        ICollection<Forum> forums = await _forumService.GetAllAsync();
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(forums);

        return res;
    }

    [Function(nameof(CreateForum))]
    [OpenApiOperation(operationId: nameof(CreateForum), tags: new[] { "Forums" }, Summary = "Create a new forum post", Description = "Creates a new forum post.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ForumDTO), Required = true, Description = "The new forum post.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ForumResponse), Description = "The forum post is created.")]
    public async Task<HttpResponseData> CreateForum([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "forums")] HttpRequestData req)
    {
        string body = await new StreamReader(req.Body).ReadToEndAsync();
        ForumDTO forumDTO = JsonConvert.DeserializeObject<ForumDTO>(body)!;
        User user = await _userService.GetUserByIdAsync(forumDTO.FromId);
        Forum forum = new(null!, forumDTO.Title, forumDTO.Description, user, null!); // @TODO: use mapper
        Forum newForum = await _forumService.CreateForum(forum);
        ForumResponse forumResponse = new(newForum.Id, newForum.Title, newForum.Description, newForum.From.Id, null!);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(forumResponse);

        return res;
    }
}
