using System.Security.Claims;
using Model.DTO;
using Model.Response;

namespace Service.Interfaces;
public interface ITokenService
{
    Task<LoginResponse> CreateToken(LoginDTO Login);
    Task<ClaimsPrincipal> GetByValue(string Value);
}
