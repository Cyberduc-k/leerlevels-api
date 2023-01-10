using System.IdentityModel.Tokens.Jwt;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.Response;
public class LoginResponse
{
    private JwtSecurityToken Token { get; }

    private JwtSecurityToken RefreshToken { get; }

    [JsonRequired]
    [OpenApiProperty(Description = "The access token to be used in every subsequent operation for this user.")]
    public string AccessToken => new JwtSecurityTokenHandler().WriteToken(Token);

    [JsonRequired]
    [OpenApiProperty(Description = "The token type.")]
    public static string TokenType => "Bearer";

    [JsonRequired]
    [OpenApiProperty(Description = "The expiration datetime of the access token")]
    public string AccessTokenExpiresAt => Token.ValidTo.ToUniversalTime().AddHours(1).ToString("dd/MM/yyyy HH:mm:ss");

    [JsonRequired]
    [OpenApiProperty(Description = "The first refresh token generated along side the initial access token. ")]
    public string InitRefreshToken => new JwtSecurityTokenHandler().WriteToken(RefreshToken);

    [JsonRequired]
    [OpenApiProperty(Description = "The expiration datetime of the initial refresh token")]
    public string InitRefreshTokenExpiresAt => RefreshToken.ValidTo.ToUniversalTime().AddHours(1).ToString("dd/MM/yyyy HH:mm:ss");

    public LoginResponse(JwtSecurityToken token, JwtSecurityToken refreshToken)
    {
        Token = token;
        RefreshToken = refreshToken;
    }
}
