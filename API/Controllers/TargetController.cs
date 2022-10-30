using System.Net;
using API.Attributes;
using API.Validators;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Model.Response;
using Service.Interfaces;

namespace API.Controllers;
public class TargetController : ControllerWithAuthentication
{
    private readonly IMapper _mapper;
    private readonly ITargetService _targetService;
    private readonly GetTargetByIdValidator validationRules;

    public TargetController(
        ILoggerFactory loggerFactory,
        ITokenService tokenService,
        ITargetService targetservice,
        IMapper mapper,
        GetTargetByIdValidator validations)
        : base(loggerFactory.CreateLogger<TargetController>(), tokenService)
    {
        _targetService = targetservice;
        _mapper = mapper;
        validationRules = validations;
    }

    // Get Targets

    [Function(nameof(GetAllTargets))]
    [OpenApiOperation(operationId: "getTargets", tags: new[] { "Targets" })]
    [OpenApiAuthentication]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Target>), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve targets.")]

    public async Task<HttpResponseData> GetAllTargets([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "targets")] HttpRequestData req)
    {
        //await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/targets");

        _logger.LogInformation("C# HTTP trigger function processed the GetUsers request.");

        ICollection<Target> targets = await _targetService.GetAllTargetsAsync();
        IEnumerable<TargetResponse> targetResponses = targets.Select(t => _mapper.Map<TargetResponse>(t));
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(targetResponses);
        return res;
    }

    // Get target

    [Function(nameof(GetTargetById))]
    [OpenApiOperation(operationId: "getTarget", tags: new[] { "Targets" })]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "targetId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The target ID parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Mcq), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Please enter a vlaid Target Id.")]

    public async Task<HttpResponseData> GetTargetById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "targets/{targetId}")] HttpRequestData req,
        string targetId)
    {
        //  await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/targets/{targetId}");

        _logger.LogInformation("C# HTTP trigger function processed a request.");

        Target targetToValidate = new() { Id = targetId };
        ValidationResult result = await validationRules.ValidateAsync(targetToValidate);

        if (result.IsValid) {
            Target target = await _targetService.GetTargetByIdAsync(targetId);
            TargetResponse targetResponse = _mapper.Map<TargetResponse>(target);
            HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

            await res.WriteAsJsonAsync(targetResponse);
            return res;
        }

        return req.CreateResponse(HttpStatusCode.BadRequest);
    }
}
