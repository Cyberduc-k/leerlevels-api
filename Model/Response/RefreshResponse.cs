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
    [OpenApiProperty(Description = "The expiration datetime of the new refresh token")]
    public string NewRefreshTokenExpiresAt => initToken.ValidTo.ToUniversalTime().AddHours(1).ToString("dd/MM/yyyy HH:mm:ss");

    [JsonRequired]
    [OpenApiProperty(Description = "The new refresh token generated based on a provided valid refresh token.")]
    public string LastRefreshToken => new JwtSecurityTokenHandler().WriteToken(lastToken);

    [JsonRequired]
    [OpenApiProperty(Description = "The expiration datetime of the latest new refresh token")]
    public string LastRefreshTokenExpiresAt => lastToken.ValidTo.ToUniversalTime().AddHours(1).ToString("dd/MM/yyyy HH:mm:ss");

    public RefreshResponse(JwtSecurityToken initRefreshToken, JwtSecurityToken lastRefreshToken)
    {
        initToken = initRefreshToken;
        lastToken = lastRefreshToken;
    }
}