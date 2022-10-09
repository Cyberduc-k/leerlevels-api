using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Model.DTO;
using Model.Response;

namespace Service.Interfaces;
public interface ITokenService
{
    Task<LoginResponse> CreateToken(LoginDTO Login);
    Task<ClaimsPrincipal> GetByValue(string Value);
}
