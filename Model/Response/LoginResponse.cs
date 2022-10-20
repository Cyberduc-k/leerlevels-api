using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Model.Response;
public class LoginResponse
{
    private JwtSecurityToken Token { get; }

    //[OpenApiProperty(Description = "The access token to be used in every subsequent operation for this user.")]
    [JsonRequired]
    public string AccessToken => new JwtSecurityTokenHandler().WriteToken(Token);

    //[OpenApiProperty(Description = "The token type.")]
    [JsonRequired]
    public static string TokenType => "Bearer";

    //[OpenApiProperty(Description = "The amount of seconds until the token expires.")]  2do: create logic for generating refresh tokens until specific point
    [JsonRequired]
    public int ExpiresIn => (int)(Token.ValidTo - DateTime.UtcNow).TotalSeconds;

    public LoginResponse(JwtSecurityToken token)
    {
        Token = token;
    }
}
