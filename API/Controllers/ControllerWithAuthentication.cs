using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Model;
using Service.Exceptions;
using Service.Interfaces;

namespace API.Controllers;

public abstract class ControllerWithAuthentication : ControllerBase
{
    protected readonly ITokenService _tokenService;

    protected ControllerWithAuthentication(ILogger logger, ITokenService tokenService) : base(logger)
    {
        _tokenService = tokenService;
    }

    public async Task ValidateAuthentication(HttpRequestData req, UserRole role, string endpoint)
    {
        // Authentication validation
        if (!await _tokenService.ValidateAuthentication(req)) {
            _logger.LogInformation($"Authentication for the {endpoint} request failed");
            throw new AuthenticationException($"{endpoint} requires authentication");
        }

        // Authorization for this endpoint
        if (_tokenService.User.Role < role) {
            _logger.LogInformation($"Authorization issue detected for the {endpoint} request");
            throw new AuthorizationException($"Must be a {role} to access {endpoint}");
        }
    }
}
