using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Azure.Functions.Worker.Http;
using Model;
using Model.DTO;
using Model.Response;

namespace Service.Interfaces;

public interface ITokenService
{
    /**
     * <summary>
     * Login using a <see cref="LoginDTO.Email"/> and <see cref="LoginDTO.Password"/>.
     * </summary>
     */
    Task<LoginResponse> Login(LoginDTO Login);

    /**
     * <summary>
     * Create a refresh token.
     * </summary>
     */
    Task<RefreshResponse> Refresh(HttpRequestData request);

    /**
     * <summary>
     * Get a <see cref="ClaimsPrincipal"/> from a JWT token.
     * </summary>
     */
    Task<ClaimsPrincipal> GetByValue(string Value);

    /**
     * <summary>
     * Valiate the request to check if it contains a Authorization header containing a valid JWT token.
     * </summary>
     */
    Task<bool> AuthenticationValidation(HttpRequestData request);

    /**
     * <summary>
     * Encrypt a password.
     * </summary>
     */
    string EncryptPassword(User user, string password);

    /**
     * <summary>
     * Get a specific claim from the JWT token in the Authorization header of the request.
     * </summary>
     */
    string GetTokenClaim(HttpRequestData req, string claim);
}
