using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;
public class RefreshResponseExample : OpenApiExample<RefreshResponse>
{
    public override IOpenApiExample<RefreshResponse> Build(NamingStrategy namingStrategy)
    {
        string Issuer = "LeerLevels";
        string Audience = "Users of the LeerLevels applications";
        TimeSpan ValidityDuration = TimeSpan.Zero;

        SigningCredentials Credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("randomstring#123455566834737!")),
            SecurityAlgorithms.HmacSha256Signature
            );

        JwtHeader Header = new(Credentials);

        List<Claim> Claims = new() {
            new Claim("userId", "1"),
            new Claim("userName", "John"),
            new Claim("userEmail", "JohnDoe@gmail.com"),
            new Claim("userRole", UserRole.Student.ToString()),
            new Claim("initTokenExpiredAt", DateTime.UtcNow.ToString()),
        };

        JwtPayload Payload = new(
            Issuer,
            Audience,
            Claims,
            DateTime.UtcNow,
            DateTime.UtcNow.Add(ValidityDuration),
            DateTime.UtcNow
        );

        Examples.Add(OpenApiExampleResolver.Resolve("Token John", new RefreshResponse(new JwtSecurityToken(Header, Payload), new JwtSecurityToken(Header, Payload))));

        return this;
    }
}