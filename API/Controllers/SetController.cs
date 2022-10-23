using System.Net;
using API.Attributes;
using API.Validation;
using API.Validators;
using FluentValidation.Results;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Service.Interfaces;

namespace API.Controllers;
public class SetController : ControllerWithAuthentication
{
    private readonly ISetService _setService;
    private readonly GetSetByIdValidator validationRules;

    public SetController(ILoggerFactory loggerFactory, ITokenService tokenService, ISetService setservice, GetSetByIdValidator validations)
        : base(loggerFactory.CreateLogger<SetController>(), tokenService)
    {
        _setService = setservice;
        validationRules = validations;  
    }

    // Get sets

    [Function(nameof(GetAllSets))]
    [OpenApiOperation(operationId: "getsets", tags: new[] { "Sets" })]
    [OpenApiAuthentication]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Set>), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve sets.")]
    public async Task<HttpResponseData> GetAllSets([HttpTrigger(AuthorizationLevel.User, "get", Route = "sets")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Teacher, "/sets");

        _logger.LogInformation("C# HTTP trigger function processed the getsets request.");

        ICollection<Set> sets = await _setService.GetAllSetsAsync();

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(sets);

        return res;
    }

    // Get set

    [Function(nameof(GetSetById))]
    [OpenApiOperation(operationId: "getSet", tags: new[] { "Sets" })]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "setId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The set ID parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Set), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Please enter a vlaid Set Id.")]
    public async Task<HttpResponseData> GetSetById([HttpTrigger(AuthorizationLevel.User, "get", Route = "sets/{setId}")] HttpRequestData req,
        string setId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/sets/{setId}");

        _logger.LogInformation("C# HTTP trigger function processed the getSet request.");

        Set SetToValidate = new() { Id = setId };
        ValidationResult result = await validationRules.ValidateAsync(SetToValidate);

        if (result.IsValid) {
            Set set = await _setService.GetSetByIdAsync(setId);

            HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

            await res.WriteAsJsonAsync(set);

            return res;
        }
        return req.CreateResponse(HttpStatusCode.BadRequest);
    }
}
