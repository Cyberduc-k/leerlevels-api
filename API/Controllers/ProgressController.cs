using System.Net;
using API.Attributes;
using API.Examples;
using API.Exceptions;
using API.Extensions;
using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Model.DTO;
using Model.Response;
using Service.Interfaces;

namespace API.Controllers;

public class ProgressController : ControllerWithAuthentication
{
    private readonly IMapper _mapper;
    private readonly IProgressService _progressService;

    public ProgressController(
        ILoggerFactory loggerFactory,
        IMapper mapper,
        ITokenService tokenService,
        IProgressService progressService
    )
        : base(loggerFactory.CreateLogger<ProgressController>(), tokenService)
    {
        _mapper = mapper;
        _progressService = progressService;
    }

    [Function(nameof(GetAllTargetProgress))]
    [OpenApiOperation(nameof(GetAllTargetProgress), tags: "Progress")]
    [OpenApiAuthentication]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "applcation/json", typeof(TargetProgressResponse[]), Example = typeof(TargetProgressResponseExample[]))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetAllTargetProgress(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "targets/progress")] HttpRequestData req)
    {
        string userId = await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/targets/progress");
        ICollection<TargetProgress> progress = await _progressService.GetAllTargetProgress(userId);
        IEnumerable<TargetProgressResponse> responses = progress.Select(p => p.CreateResponse());
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(responses);
        return res;
    }

    [Function(nameof(GetTargetProgress))]
    [OpenApiOperation(nameof(GetTargetProgress), tags: "Progress")]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "targetId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The target ID parameter")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "applcation/json", typeof(TargetProgressResponse), Example = typeof(TargetProgressResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the target progress with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetTargetProgress(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "targets/{targetId}/progress")] HttpRequestData req,
        string targetId)
    {
        string userId = await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/targets/{targetId}/progress");
        TargetProgress targetProgress = await _progressService.GetTargetProgress(targetId, userId);
        TargetProgressResponse targetProgressResponse = targetProgress.CreateResponse();
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(targetProgressResponse);
        return res;
    }

    [Function(nameof(BeginTarget))]
    [OpenApiOperation(nameof(BeginTarget), tags: "Progress")]
    [OpenApiAuthentication]
    [OpenApiParameter("targetId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The target ID parameter")]
    [OpenApiParameter("reset", In = ParameterLocation.Query, Required = false, Type = typeof(bool))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "applcation/json", typeof(TargetProgressResponse), Example = typeof(TargetProgressResponseExample))]
    [OpenApiResponseWithBody(HttpStatusCode.Created, "applcation/json", typeof(TargetProgressResponse), Example = typeof(TargetProgressResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the target with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> BeginTarget(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "targets/{targetId}/progress")] HttpRequestData req,
        string targetId)
    {
        string userId = await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/targets/{targetId}/progress");
        bool reset = req.Query("reset").GetBool() ?? throw new InvalidQueryParameterException("reset");
        (TargetProgress targetProgress, bool created) = await _progressService.BeginTarget(targetId, userId, reset);
        TargetProgressResponse targetProgressResponse = targetProgress.CreateResponse();
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(targetProgressResponse);
        if (created) res.StatusCode = HttpStatusCode.Created;
        return res;
    }

    [Function(nameof(GetMcqProgress))]
    [OpenApiOperation(nameof(GetMcqProgress), tags: "Progress")]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "mcqId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The mcq ID parameter")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "applcation/json", typeof(McqProgressResponse), Example = typeof(McqProgressResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the mcq progress with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetMcqProgress(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "mcqs/{mcqId}/progress")] HttpRequestData req,
        string mcqId)
    {
        string userId = await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/mcqs/{mcqId}/progress");
        McqProgress mcqProgress = await _progressService.GetMcqProgress(mcqId, userId);
        McqProgressResponse mcqProgressResponse = mcqProgress.CreateResponse();
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(mcqProgressResponse);
        return res;
    }

    [Function(nameof(AnswerQuestion))]
    [OpenApiOperation(nameof(AnswerQuestion), tags: "Progress")]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "mcqId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The mcq ID parameter")]
    [OpenApiRequestBody("application/json", typeof(McqProgressDTO), Required = true, Example = typeof(McqProgressDTOExample))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "applcation/json", typeof(McqProgressResponse), Example = typeof(McqProgressResponseExample))]
    [OpenApiResponseWithBody(HttpStatusCode.Created, "applcation/json", typeof(McqProgressResponse), Example = typeof(McqProgressResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the mcq with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> AnswerQuestion(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "mcqs/{mcqId}/progress")] HttpRequestData req,
        string mcqId)
    {
        string userId = await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/mcqs/{mcqId}/progress");
        McqProgressDTO? mcqProgressDTO = await req.ReadFromJsonAsync<McqProgressDTO>();
        (McqProgress mcqProgress, bool created) = await _progressService.AnswerQuestion(
            mcqId,
            mcqProgressDTO!.AnswerOptionId,
            mcqProgressDTO.AnswerKind,
            userId);
        McqProgressResponse mcqProgressResponse = mcqProgress.CreateResponse();
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(mcqProgressResponse);
        if (created) res.StatusCode = HttpStatusCode.Created;
        return res;
    }

    [Function(nameof(GetSetProgress))]
    [OpenApiOperation(nameof(GetSetProgress), tags: "Progress")]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "setId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The set ID parameter")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "applcation/json", typeof(SetProgressResponse), Example = typeof(SetProgressResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the set with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetSetProgress(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "sets/{setId}/progress")] HttpRequestData req,
        string setId)
    {
        string userId = await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/sets/{setId}/progress");
        SetProgress setProgress = await _progressService.GetSetProgress(setId, userId);
        SetProgressResponse setProgresResponse = setProgress.CreateResponse();
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(setProgresResponse);
        return res;
    }

    [Function(nameof(GetUserProgress))]
    [OpenApiOperation(nameof(GetUserProgress), tags: "Users", Summary = "The progress of the user", Description = "Returns the progress of the curent user in all targets and sets.")]
    [OpenApiAuthentication]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserProgressResponse), Description = "A progres of the user")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetUserProgress([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/progress")] HttpRequestData req)
    {
        string userId = await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/user/progress");
        UserProgress userProgress = await _progressService.GetUserProgress(userId);
        UserProgressResponse userProgressResponse = userProgress.CreateResponse();
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(userProgressResponse);
        return res;
    }
}
