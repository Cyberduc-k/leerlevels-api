using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.DTO;
using Model.Response;
using Repository.Interfaces;
using Service.Exceptions;
using Service.Interfaces;

namespace Service;

public class TokenService : ITokenService
{
    private ILogger Logger { get; }
    private  IUserRepository UserRepository { get; set; }
    private string Issuer { get; }
    private string Audience { get; }
    private TimeSpan ValidityDuration { get; }
    private SigningCredentials Credentials { get; }
    private TokenIdentityValidationParameters ValidationParameters { get; }

    public TokenService(IConfiguration Configuration, ILogger<TokenService> Logger, IUserRepository userRepository)
    {
        this.Logger = Logger;

        UserRepository = userRepository;

        Issuer = Configuration["LeerLevels"] ?? "DebugIssuer";
        Audience = Configuration["Users of the LeerLevels application"] ?? "DebugIssuer"; /*Configuration.GetClassValueChecked("JWT:Audience", "DebugAudience";//, Logger);*/
        ValidityDuration = TimeSpan.FromDays(1);// Todo: configure an appropriate validity duration (2 hours and then generate refresh tokens? read from config somewhere when another login is required/so until how long refresh tokens are generated after init login)
        string Key = Configuration["The LeerLevels JWT token keys tring used for authentication and authorization purposes"] ?? "DebugKey DebugKey that is a long size by default"; /*Configuration.GetClassValueChecked("JWT:Key", "DebugKey DebugKey";//, Logger);*/

        SymmetricSecurityKey SecurityKey = new(Encoding.UTF8.GetBytes(Key));

        Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);

        ValidationParameters = new TokenIdentityValidationParameters(Issuer, Audience, SecurityKey);
    }

    public async Task<LoginResponse> Login(LoginDTO loginDTO)
    {
        //authentication of the login information
        User user = await UserRepository.GetUserByLoginInfo(loginDTO.UserName, loginDTO.Password) ?? throw new NotFoundException("user to create a valid token");

        JwtSecurityToken Token = await CreateToken(user);

        return new LoginResponse(Token);
    }

    private async Task<JwtSecurityToken> CreateToken(User user)
    {
        JwtHeader Header = new(Credentials);

        Claim[] Claims = new Claim[] {
            new Claim("userId", user.Id),
            new Claim("userName", user.UserName),
            new Claim("userRole", user.Role.ToString()),
        };

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
            ClaimsPrincipal Principal = Handler.ValidateToken(Value, ValidationParameters, out SecurityToken ValidatedToken);

            return await Task.FromResult(Principal);
        } catch (Exception e) {
            Logger.LogInformation(e.Message);
            throw;
        }
    }
}
