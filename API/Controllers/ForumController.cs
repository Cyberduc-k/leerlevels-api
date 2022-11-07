using System.Net;
using API.Attributes;
using API.Examples;
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

public class ForumController : ControllerWithAuthentication
{
    private readonly IMapper _mapper;
    private readonly IForumService _forumService;

    public ForumController(ILoggerFactory loggerFactory, ITokenService tokenService, IMapper mapper, IForumService forumService)
        : base(loggerFactory.CreateLogger<ForumController>(), tokenService)
    {
        _mapper = mapper;
        _forumService = forumService;
    }

    [Function(nameof(GetForums))]
    [OpenApiOperation(operationId: nameof(GetForums), tags: new[] { "Forums" }, Summary = "A list of forum posts", Description = "Returns a list of all forum posts")]
    [OpenApiAuthentication]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ForumResponse[]), Description = "A list of forum posts", Example = typeof(ForumResponseExample[]))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetForums([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "forums")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/forums");
        ICollection<Forum> forums = await _forumService.GetAll();
        IEnumerable<ForumResponse> forumResponses = forums.Select(f => _mapper.Map<ForumResponse>(f));
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(forumResponses);

        return res;
    }

    [Function(nameof(CreateForum))]
    [OpenApiOperation(operationId: nameof(CreateForum), tags: new[] { "Forums" }, Summary = "Create a new forum post", Description = "Creates a new forum post")]
    [OpenApiAuthentication]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ForumDTO), Required = true, Description = "The new forum post", Example = typeof(ForumDTOExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ForumResponse), Description = "The forum post is created", Example = typeof(ForumResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> CreateForum([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "forums")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/forums");
        string body = await new StreamReader(req.Body).ReadToEndAsync();
        ForumDTO forumDTO = JsonConvert.DeserializeObject<ForumDTO>(body)!;
        forumDTO.FromId ??= _tokenService.User.Id;
        Forum forum = await _mapper.Map<Task<Forum>>(forumDTO);
        Forum newForum = await _forumService.CreateForum(forum);
        ForumResponse forumResponse = _mapper.Map<ForumResponse>(newForum);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(forumResponse);

        return res;
    }

    [Function(nameof(GetForum))]
    [OpenApiOperation(operationId: nameof(GetForum), tags: new[] { "Forums" }, Summary = "A single forum post", Description = "Returns a single forum post")]
    [OpenApiAuthentication]
    [OpenApiParameter("forumId", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The forum post Id")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ForumResponse), Description = "A forum post", Example = typeof(ForumResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the forum post with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetForum(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "forums/{forumId}")] HttpRequestData req,
        string forumId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/forums/{forumId}");
        Forum forum = await _forumService.GetById(forumId);
        ForumResponse forumResponse = _mapper.Map<ForumResponse>(forum);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(forumResponse);

        return res;
    }

    [Function(nameof(UpdateForum))]
    [OpenApiOperation(operationId: nameof(UpdateForum), tags: new[] { "Forums" }, Summary = "Edit a forum post", Description = "Edits a forum post")]
    [OpenApiAuthentication]
    [OpenApiParameter("forumId", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The forum post Id")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateForumDTO), Required = true, Description = "The edited forum post", Example = typeof(UpdateForumDTOExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The forum post is edited")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the forum post with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> UpdateForum(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "forums/{forumId}")] HttpRequestData req,
        string forumId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/forums/{forumId}");
        string body = await new StreamReader(req.Body).ReadToEndAsync();
        UpdateForumDTO forumDTO = JsonConvert.DeserializeObject<UpdateForumDTO>(body)!;

        await _forumService.UpdateForum(forumId, forumDTO);

        return req.CreateResponse(HttpStatusCode.OK);
    }

    [Function(nameof(CreateForumReply))]
    [OpenApiOperation(operationId: nameof(CreateForumReply), tags: new[] { "Forums" }, Summary = "Add a reply to a forum post", Description = "Adds a new reply to a forum post")]
    [OpenApiAuthentication]
    [OpenApiParameter("forumId", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The forum post Id")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ForumReplyDTO), Required = true, Description = "The new forum post reply", Example = typeof(ForumReplyDTOExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(ForumReplyResponse), Description = "The forum post reply is created", Example = typeof(ForumReplyResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the forum post with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> CreateForumReply(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "forums/{forumId}/replies")] HttpRequestData req,
        string forumId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/forums/{forumId}/replies");
        string body = await new StreamReader(req.Body).ReadToEndAsync();
        ForumReplyDTO forumReplyDTO = JsonConvert.DeserializeObject<ForumReplyDTO>(body)!;
        forumReplyDTO.FromId ??= _tokenService.User.Id;
        ForumReply forumReply = await _mapper.Map<Task<ForumReply>>(forumReplyDTO);
        ForumReply newForumReply = await _forumService.AddReply(forumId, forumReply);
        ForumReplyResponse forumReplyResponse = _mapper.Map<ForumReplyResponse>(newForumReply);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(forumReplyResponse);

        return res;
    }

    [Function(nameof(UpdateForumReply))]
    [OpenApiOperation(operationId: nameof(UpdateForumReply), tags: new[] { "Forums" }, Summary = "Edit a forum post reply", Description = "Edits a forum post reply")]
    [OpenApiAuthentication]
    [OpenApiParameter("forumId", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The forum post Id")]
    [OpenApiParameter("replyId", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The forum post reply Id")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateForumReplyDTO), Required = true, Description = "The edited forum post reply", Example = typeof(UpdateForumReplyDTOExample))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The forum post reply is edited")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the forum post reply with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> UpdateForumReply(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "forums/{forumId}/replies/{replyId}")] HttpRequestData req,
        string replyId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/forums/{forumId}/replies/{replyId}");
        string body = await new StreamReader(req.Body).ReadToEndAsync();
        UpdateForumReplyDTO forumReplyDTO = JsonConvert.DeserializeObject<UpdateForumReplyDTO>(body)!;

        await _forumService.UpdateForumReply(replyId, forumReplyDTO);

        return req.CreateResponse(HttpStatusCode.OK);
    }

    [Function(nameof(DeleteForumReply))]
    [OpenApiOperation(operationId: nameof(DeleteForumReply), tags: new[] { "Forums" }, Summary = "Delete a forum post reply", Description = "Deletes a forum post reply")]
    [OpenApiAuthentication]
    [OpenApiParameter("forumId", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The forum post Id")]
    [OpenApiParameter("replyId", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The forum post reply Id")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The forum post reply is deleted")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the forum post or reply with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> DeleteForumReply(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "forums/{forumId}/replies/{replyId}")] HttpRequestData req,
        string forumId,
        string replyId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/forums/{forumId}/replies/{replyId}");
        await _forumService.DeleteForumReply(forumId, replyId);

        return req.CreateResponse(HttpStatusCode.OK);
    }
}
