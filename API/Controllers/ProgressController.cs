using System.Net;
using API.Attributes;
using AutoMapper;
using Microsoft.Azure.Functions.Worker;
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
    [OpenApiResponseWithBody(HttpStatusCode.OK, "applcation/json", typeof(TargetProgressResponse[]))]
    public async Task<HttpResponseData> GetAllTargetProgress(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "targets/progress")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/targets/progress");
        ICollection<TargetProgress> progress = await _progressService.GetAllTargetProgress();
        IEnumerable<TargetProgressResponse> responses = progress.Select(p => p.CreateResponse());
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(responses);
        return res;
    }

    [Function(nameof(GetTargetProgress))]
    [OpenApiOperation(nameof(GetTargetProgress), tags: "Progress")]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "targetId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The target ID parameter")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "applcation/json", typeof(TargetProgressResponse))]
    public async Task<HttpResponseData> GetTargetProgress(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "targets/{targetId}/progress")] HttpRequestData req,
        string targetId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/targets/{targetId}/progress");
        TargetProgress targetProgress = await _progressService.GetTargetProgress(targetId);
        TargetProgressResponse targetProgressResponse = targetProgress.CreateResponse();
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(targetProgressResponse);
        return res;
    }

    [Function(nameof(BeginTarget))]
    [OpenApiOperation(nameof(BeginTarget), tags: "Progress")]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "targetId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The target ID parameter")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "applcation/json", typeof(TargetProgressResponse))]
    public async Task<HttpResponseData> BeginTarget(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "targets/{targetId}/progress")] HttpRequestData req,
        string targetId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/targets/{targetId}/progress");
        TargetProgress targetProgress = await _progressService.BeginTarget(targetId);
        TargetProgressResponse targetProgressResponse = targetProgress.CreateResponse();
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(targetProgressResponse);
        return res;
    }

    [Function(nameof(GetMcqProgress))]
    [OpenApiOperation(nameof(GetMcqProgress), tags: "Progress")]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "mcqId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The mcq ID parameter")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "applcation/json", typeof(McqProgressResponse))]
    public async Task<HttpResponseData> GetMcqProgress(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "mcqs/{mcqId}/progress")] HttpRequestData req,
        string mcqId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/mcqs/{mcqId}/progress");
        McqProgress mcqProgress = await _progressService.GetMcqProgress(mcqId);
        McqProgressResponse mcqProgressResponse = mcqProgress.CreateResponse();
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(mcqProgressResponse);
        return res;
    }

    [Function(nameof(AnswerQuestion))]
    [OpenApiOperation(nameof(AnswerQuestion), tags: "Progress")]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "mcqId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The mcq ID parameter")]
    [OpenApiRequestBody("application/json", typeof(McqProgressDTO), Required = true)]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "applcation/json", typeof(McqProgressResponse))]
    public async Task<HttpResponseData> AnswerQuestion(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "mcqs/{mcqId}/progress")] HttpRequestData req,
        string mcqId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/mcqs/{mcqId}/progress");
        McqProgressDTO? mcqProgressDTO = await req.ReadFromJsonAsync<McqProgressDTO>();
        McqProgress mcqProgress = await _progressService.AnswerQuestion(mcqId, mcqProgressDTO!.AnswerOptionId, mcqProgressDTO.AnswerKind);
        McqProgressResponse mcqProgressResponse = mcqProgress.CreateResponse();
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(mcqProgressResponse);
        return res;
    }

    [Function(nameof(GetSetProgress))]
    [OpenApiOperation(nameof(GetSetProgress), tags: "Progress")]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "setId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The set ID parameter")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "applcation/json", typeof(SetProgressResponse))]
    public async Task<HttpResponseData> GetSetProgress(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "sets/{setId}/progress")] HttpRequestData req,
        string setId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/sets/{setId}/progress");
        SetProgress setProgress = await _progressService.GetSetProgress(setId);
        SetProgressResponse setProgresResponse = setProgress.CreateResponse();
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(setProgresResponse);
        return res;
    }
}
