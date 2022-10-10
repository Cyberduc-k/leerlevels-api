using System.Net;
using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Model.DTO;
using Model.Response;
using Newtonsoft.Json;
using Service.Interfaces;

namespace API.Controllers;

public class ForumController
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IForumService _forumService;

    public ForumController(ILoggerFactory loggerFactory, IMapper mapper, IForumService forumService)
    {
        _logger = loggerFactory.CreateLogger<ForumController>();
        _mapper = mapper;
        _forumService = forumService;
    }

    [Function(nameof(GetForums))]
    [OpenApiOperation(operationId: nameof(GetForums), tags: new[] { "Forums" }, Summary = "A list of forum posts", Description = "Returns a list of all forum posts")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ForumResponse[]), Description = "A list of forum posts")]
    public async Task<HttpResponseData> GetForums([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "forums")] HttpRequestData req)
    {
        ICollection<Forum> forums = await _forumService.GetAllAsync();
        IEnumerable<ForumResponse> forumResponses = forums.Select(f => _mapper.Map<ForumResponse>(f));
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(forumResponses);

        return res;
    }

    [Function(nameof(CreateForum))]
    [OpenApiOperation(operationId: nameof(CreateForum), tags: new[] { "Forums" }, Summary = "Create a new forum post", Description = "Creates a new forum post")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ForumDTO), Required = true, Description = "The new forum post")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ForumResponse), Description = "The forum post is created")]
    public async Task<HttpResponseData> CreateForum([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "forums")] HttpRequestData req)
    {
        string body = await new StreamReader(req.Body).ReadToEndAsync();
        ForumDTO forumDTO = JsonConvert.DeserializeObject<ForumDTO>(body)!;
        Forum forum = await _mapper.Map<Task<Forum>>(forumDTO);
        Forum newForum = await _forumService.CreateForum(forum);
        ForumResponse forumResponse = _mapper.Map<ForumResponse>(newForum);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(forumResponse);

        return res;
    }

    [Function(nameof(GetForum))]
    [OpenApiOperation(operationId: nameof(GetForum), tags: new[] { "Forums" }, Summary = "A single forum post", Description = "Returns a single forum post")]
    [OpenApiParameter("forumId", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The forum post id Id")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ForumResponse), Description = "A forum post")]
    public async Task<HttpResponseData> GetForum(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "forums/{forumId}")] HttpRequestData req,
        string forumId)
    {
        Forum? forum = await _forumService.GetByIdAsync(forumId);
        ForumResponse forumResponse = _mapper.Map<ForumResponse>(forum);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(forumResponse);

        return res;
    }
}
