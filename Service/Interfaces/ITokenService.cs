using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Azure.Functions.Worker.Http;
using Model;
using Model.DTO;
using Model.Response;

namespace Service.Interfaces;
public interface ITokenService
{
    Task<LoginResponse> Login(LoginDTO Login);
    Task<JwtSecurityToken> CreateToken(User user);
    Task<ClaimsPrincipal> GetByValue(string Value);
    Task<bool> ValidateAuthentication(HttpRequestData request);
}
