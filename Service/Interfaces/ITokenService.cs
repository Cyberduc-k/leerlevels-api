using System.Security.Claims;
using Model.DTO;
using Model.Response;

namespace Service.Interfaces;
public interface ITokenService
{
    Task<LoginResponse> Login(LoginDTO Login);
    Task<ClaimsPrincipal> GetByValue(string Value);
}
