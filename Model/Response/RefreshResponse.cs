using System.IdentityModel.Tokens.Jwt;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.Response;
public class RefreshResponse
{
    private JwtSecurityToken initToken { get; }

    private JwtSecurityToken lastToken { get; }

    [JsonRequired]
    [OpenApiProperty(Description = "The new refresh token generated based on a provided valid refresh token.")]
    public string NewRefreshToken => new JwtSecurityTokenHandler().WriteToken(initToken);

    [JsonRequired]
    [OpenApiProperty(Description = "The new refresh token generated based on a provided valid refresh token.")]
    public string LastRefreshToken => new JwtSecurityTokenHandler().WriteToken(lastToken);

    public RefreshResponse(JwtSecurityToken initRefreshToken, JwtSecurityToken lastRefreshToken)
    {
        initToken = initRefreshToken;
        lastToken = lastRefreshToken;
    }
}