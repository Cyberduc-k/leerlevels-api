using System.IdentityModel.Tokens.Jwt;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.Response;
public class RefreshResponse
{
    private JwtSecurityToken Token { get; }

    [JsonRequired]
    [OpenApiProperty(Description = "The new refresh token generated based on a provided valid refresh token.")]
    public string RefreshToken => new JwtSecurityTokenHandler().WriteToken(Token);

    public RefreshResponse(JwtSecurityToken refreshToken)
    {
        Token = refreshToken;
    }
}
