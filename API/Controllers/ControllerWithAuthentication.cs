using System.Net;
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

    public async Task<string> ValidateAuthenticationAndAuthorization(HttpRequestData req, UserRole role, string endpoint)
    {
        // Authentication validation
        if (!await _tokenService.AuthenticationValidation(req)) {
            _logger.LogInformation($"Authentication for the {endpoint} request failed");
            throw new AuthenticationException($"{endpoint} requires authentication, error: {_tokenService.Message}");
        }

        // Authorization for this endpoint
        Enum.TryParse(_tokenService.GetTokenClaim(req, "userRole"), out UserRole tokenUserRole);
        if (tokenUserRole < role) {
            _logger.LogInformation($"Authorization issue detected for the {endpoint} request");
            throw new AuthorizationException($"Must be a {role} to access {endpoint}");
        }

        return _tokenService.GetTokenClaim(req, "userId");
    }
}