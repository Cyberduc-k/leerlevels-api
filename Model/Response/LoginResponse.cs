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
    [OpenApiProperty(Description = "The amount of seconds until the token expires.")]
    public int ExpiresIn => (int)(Token.ValidTo - DateTime.UtcNow).TotalSeconds;

    [JsonRequired]
    [OpenApiProperty(Description = "The first refresh token generated along side the initial access token. ")]
    public string InitRefreshToken => new JwtSecurityTokenHandler().WriteToken(RefreshToken);

    public LoginResponse(JwtSecurityToken token, JwtSecurityToken refreshToken)
    {
        Token = token;
        RefreshToken = refreshToken;
    }
}
