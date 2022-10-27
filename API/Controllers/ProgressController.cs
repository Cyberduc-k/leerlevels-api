using API.Attributes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Service.Interfaces;

namespace API.Controllers;

public class ProgressController : ControllerWithAuthentication
{
    private readonly IProgressService _progressService;
    private readonly ITargetService _targetService;

    public ProgressController(ILoggerFactory loggerFactory, ITokenService tokenService, IProgressService progressService, ITargetService targetService)
        : base(loggerFactory.CreateLogger<ProgressController>(), tokenService)
    {
        _progressService = progressService;
        _targetService = targetService;
    }

    [Function(nameof(BeginTarget))]
    [OpenApiOperation(nameof(BeginTarget))]
    [OpenApiAuthentication]
    [OpenApiRequestBody("application/json", typeof(TargetProgressDTO), Required = true)]
    public async Task<HttpResponseData> BeginTarget(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "progress/target")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/progress/target");
        TargetProgressDTO? targetProgressDTO = await req.ReadFromJsonAsync<TargetProgressDTO>();
        Target target = await _targetService.GetTargetWithMcqByIdAsync(targetProgressDTO!.TargetId);

        await _progressService.BeginTarget(target);
        return req.CreateResponse();
    }
}
