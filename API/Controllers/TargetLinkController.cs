using System.Net;
using API.Attributes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Service.Interfaces;

namespace API.Controllers;

public class TargetLinkController : ControllerWithAuthentication
{
    private readonly ITargetLinkService _targetLinkService;

    public TargetLinkController(
        ILoggerFactory loggerFactory,
        ITokenService tokenService,
        ITargetLinkService targetLinkService)
        : base(loggerFactory.CreateLogger<TargetLinkController>(), tokenService)
    {
        _targetLinkService = targetLinkService;
    }

    [Function(nameof(GetNextTargets))]
    [OpenApiOperation(nameof(GetNextTargets), tags: "Targets")]
    [OpenApiAuthentication]
    [OpenApiParameter("targetId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The target ID parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string[]), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Please enter a valid Target Id.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the Target with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetNextTargets(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "targets/{targetId}/next")] HttpRequestData req,
        string targetId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/targets/{targetId}/next");
        ICollection<string> targetIds = await _targetLinkService.GetNextTargets(targetId);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(targetIds);
        return res;
    }

    [Function(nameof(GetPrevTargets))]
    [OpenApiOperation(nameof(GetPrevTargets), tags: "Targets")]
    [OpenApiAuthentication]
    [OpenApiParameter("targetId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The target ID parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string[]), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Please enter a valid Target Id.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the Target with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetPrevTargets(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "targets/{targetId}/previous")] HttpRequestData req,
        string targetId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/targets/{targetId}/previous");
        ICollection<string> targetIds = await _targetLinkService.GetPrevTargets(targetId);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(targetIds);
        return res;
    }
}
