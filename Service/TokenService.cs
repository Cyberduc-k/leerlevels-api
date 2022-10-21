using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.DTO;
using Model.Response;
using Service.Interfaces;

namespace Service;

public class TokenService : ITokenService
{
    private ILogger Logger { get; }
    private string Issuer { get; }
    private string Audience { get; }
    private TimeSpan ValidityDuration { get; }
    private SigningCredentials Credentials { get; }
    private TokenIdentityValidationParameters ValidationParameters { get; }

    public TokenService(IConfiguration Configuration, ILogger<TokenService> Logger)
    {
        this.Logger = Logger;

        Issuer = Configuration["JWT:Issuer"] ?? "DebugIssuer";
        Audience = Configuration["JWT:Audience"] ?? "DebugIssuer"; /*Configuration.GetClassValueChecked("JWT:Audience", "DebugAudience";//, Logger);*/
        ValidityDuration = TimeSpan.FromDays(1);// Todo: configure an appropriate validity duration (2 hours and then generate refresh tokens? read from config somewhere when another login is required/so until how long refresh tokens are generated after init login)
        string Key = Configuration["JWT:Key"] ?? "DebugKey DebugKey"; /*Configuration.GetClassValueChecked("JWT:Key", "DebugKey DebugKey";//, Logger);*/

        SymmetricSecurityKey SecurityKey = new(Encoding.UTF8.GetBytes(Key));

        Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);

        ValidationParameters = new TokenIdentityValidationParameters(Issuer, Audience, SecurityKey);
    }

    public async Task<LoginResponse> CreateToken(LoginDTO Login)
    {
        JwtSecurityToken Token = await CreateToken(new Claim[] {
            new Claim(ClaimTypes.Role, "User" /*UserRole.Student.ToString()*/),
            new Claim(ClaimTypes.Name, Login.UserName)
        });

        return new LoginResponse(Token);
    }

    private async Task<JwtSecurityToken> CreateToken(Claim[] Claims)
    {
        JwtHeader Header = new(Credentials);

        JwtPayload Payload = new(
            Issuer,
            Audience,
            Claims,
            DateTime.UtcNow,
            DateTime.UtcNow.Add(ValidityDuration),
            DateTime.UtcNow
        );

        JwtSecurityToken SecurityToken = new(Header, Payload);

        return await Task.FromResult(SecurityToken);
    }

    public async Task<ClaimsPrincipal> GetByValue(string Value)
    {
        if (Value == null) {
            throw new Exception("No Token supplied");
        }

        JwtSecurityTokenHandler Handler = new();

        try {
            SecurityToken ValidatedToken;
            ClaimsPrincipal Principal = Handler.ValidateToken(Value, ValidationParameters, out ValidatedToken);

            return await Task.FromResult(Principal);
        } catch (Exception e) {
            Logger.LogInformation(e.Message);
            throw;
        }
    }
}
