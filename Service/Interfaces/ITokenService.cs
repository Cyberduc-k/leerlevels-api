using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Azure.Functions.Worker.Http;
using Model;
using Model.DTO;
using Model.Response;

namespace Service.Interfaces;
public interface ITokenService
{
    string Message { get; set; }

    Task<LoginResponse> Login(LoginDTO Login);
    Task<JwtSecurityToken> CreateToken(User user);
    Task<ClaimsPrincipal> GetByValue(string Value);
    Task<bool> AuthenticationValidation(HttpRequestData request);
    string EncryptPassword(User user, string password);
    bool IsHashSupported(string hashString);
    Task<bool> VerifyPassword(User user, string storedPassword, string password);
    string GetTokenClaim(HttpRequestData req, string claim);
}
