using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.DTO;
using Model.Response;
using Repository.Interfaces;
using Service.Exceptions;
using Service.Interfaces;
using YamlDotNet.Core.Tokens;

namespace Service;

public class TokenService : ITokenService
{
    private ILogger Logger { get; }
    private IUserRepository UserRepository { get; set; }
    private string Issuer { get; }
    private string Audience { get; }
    private TimeSpan ValidityDuration { get; set; }
    private SigningCredentials Credentials { get; }
    private TokenIdentityValidationParameters ValidationParameters { get; }

    private PasswordHasher<User> PasswordHasher { get; }

    public string Message { get; set; }

    public TokenService(ILoggerFactory loggerFactory, IUserRepository userRepository)
    {
        Logger = loggerFactory.CreateLogger<TokenService>();

        UserRepository = userRepository;

        Issuer = "LeerLevels";
        Audience = "Users of the LeerLevels applications";

        ValidityDuration = TimeSpan.FromHours(2);


        string Key = Environment.GetEnvironmentVariable("LeerLevelsTokenKey")!;

        SymmetricSecurityKey SecurityKey = new(Encoding.UTF8.GetBytes(Key));

        Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);

        ValidationParameters = new TokenIdentityValidationParameters(Issuer, Audience, SecurityKey);

        PasswordHasher = new PasswordHasher<User>();

    }

    public async Task<LoginResponse> Login(LoginDTO loginDTO)
    {
        //authentication of the login information
        User user = await UserRepository.GetByAsync(u => u.Email == loginDTO.Email) ?? throw new NotFoundException("user to create a valid token");

        //check if given password is valid for the saved hash of the users password
        if (!await VerifyPassword(user, user.Password, loginDTO.Password)) {
            throw new AuthenticationException("Incorrect user password entered");
        }

        user.LastLogin = DateTime.UtcNow;

        await UserRepository.SaveChanges();

        JwtSecurityToken Token = await CreateToken(user, "no", DateTime.UtcNow.ToString());

        JwtSecurityToken refreshToken = await CreateToken(user, "yes", DateTime.UtcNow.ToString());

        return new LoginResponse(Token, refreshToken);
    }

    public async Task<RefreshResponse> Refresh(HttpRequestData request)
    {
        User user = await UserRepository.GetByIdAsync(GetTokenClaim(request, "userId")) ?? throw new NotFoundException("user to creat a valid refresh token");

        JwtSecurityToken initRefreshToken = await CreateToken(user, "refresh", GetTokenClaim(request, "initTokenExpiredAt"));

        JwtSecurityToken lastRefreshToken = await CreateToken(user, "nextrefresh", GetTokenClaim(request, "initTokenExpiredAt"));

        return new RefreshResponse(initRefreshToken, lastRefreshToken);
    }

    public async Task<JwtSecurityToken> CreateToken(User user, string refreshTokenPhrase, string initialTokenExpiration)
    {
        JwtHeader Header = new(Credentials);

        List<Claim> Claims = new() {
            new Claim("userId", user.Id),
            new Claim("userName", user.UserName), 
            new Claim("userEmail", user.Email),
            new Claim("userRole", user.Role.ToString()),
        };

        if (refreshTokenPhrase == "yes") {
            ValidityDuration = TimeSpan.FromHours(2.25); //the initial refresh token is valid for 15 minutes longer than the initial token to allow for a call to refresh this token
            Claims.Add(new Claim("initTokenExpiredAt", DateTime.UtcNow.Add(ValidityDuration).ToString()));
        } else if (refreshTokenPhrase == "refresh") {
            ValidityDuration = TimeSpan.FromHours(2.25);
            Claims.Add(new Claim("initTokenExpiredAt", initialTokenExpiration));
        } else if (refreshTokenPhrase == "nextrefresh") {
	        ValidityDuration = TimeSpan.FromHours(2.5);
            Claims.Add(new Claim("initTokenExpiredAt", initialTokenExpiration));
	}
	else if (refreshTokenPhrase == "no") {
            ValidityDuration = TimeSpan.FromHours(2);
        }

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
            throw new AuthenticationException("No JWT type token was supplied");
        }

        JwtSecurityTokenHandler Handler = new();

        try {
            ClaimsPrincipal Principal = Handler.ValidateToken(Value, ValidationParameters, out SecurityToken ValidatedToken);

            return await Task.FromResult(Principal);
        } catch (Exception e) {
            Logger.LogInformation(e.Message);
            throw new AuthenticationException(e.Message);
        }
    }

    public async Task<bool> AuthenticationValidation(HttpRequestData req)
    {
        // check if authorization header exists (also done in the JwtMiddleware)
        if (!req.Headers.Contains("Authorization")) {
            return false;
        }

        // get headers as dictionary
        Dictionary<string, string> headers = req.Headers.ToDictionary(h => h.Key, h => string.Join(";", h.Value));

        if (string.IsNullOrEmpty(headers["Authorization"])) {
            return false;
        }

        AuthenticationHeaderValue BearerHeader = AuthenticationHeaderValue.Parse(headers["Authorization"]);

        // validation of token based on the validation parameters (also done in the JwtMiddleware)
        ClaimsPrincipal Token = await GetByValue(BearerHeader.Parameter!);

        if (Token == null || !Token.Claims.Any()) {
            return false;
        }

        Dictionary<string, string> claims = Token.Claims.ToDictionary(t => t.Type, t => t.Value);

        // validation of token set/presence of user related claims (id, name and role)
        if (!claims.ContainsKey("userId") || !claims.ContainsKey("userName") || !claims.ContainsKey("userEmail") || !claims.ContainsKey("userRole")) {
            return false;
        }

        // validation of token issuer and audience
        if (claims["iss"] != "LeerLevels" || claims["aud"] != "Users of the LeerLevels applications") {
            return false;
        }

        // validation of the presence of token validity claims (not before, expires at & issued at)
        if (!claims.ContainsKey("nbf") || !claims.ContainsKey("exp") || !claims.ContainsKey("iat")) {
            return false;
        }

        // validation of refresh token specifically: check if the initial token expried over a year ago and a new login is required or not
        if (claims.ContainsKey("initTokenExpiredAt")) {
            DateTime initialTokenExpiredAt = DateTime.Parse(claims["initTokenExpiredAt"]);

            if (initialTokenExpiredAt < DateTime.UtcNow.AddYears(-1)) { // if the token expired over a year ago then return false here
                return false;
            }
        }

        // validation of token expiration (this is already done by the ValidateToken method but we might want to implement this here again, for additional safety)
        if (claims["nbf"] != null && claims["exp"] != null && claims["iat"] != null) {

            DateTime notValidBefore = DateTimeOffset.FromUnixTimeSeconds(long.Parse(claims["nbf"])).UtcDateTime;
            DateTime expiresAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(claims["exp"])).UtcDateTime;
            DateTime issuedAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(claims["iat"])).UtcDateTime;

            if (issuedAt > DateTime.UtcNow || notValidBefore > DateTime.UtcNow || expiresAt < DateTime.UtcNow) {
                return false;
            }
        } else {
            return false;
        }

        User user = await UserRepository.GetByIdAsync(claims["userId"]) ?? throw new NotFoundException("user");

        if (!user!.IsActive) {
            return false;
        }

        return true;
    }

    public string GetTokenClaim(HttpRequestData req, string claim)
    {
        // get headers as dictionary
        Dictionary<string, string> headers = req.Headers.ToDictionary(h => h.Key, h => string.Join(";", h.Value));

        //get the authorization header value (the token itself)
        AuthenticationHeaderValue BearerHeader = AuthenticationHeaderValue.Parse(headers["Authorization"]);

        JwtSecurityTokenHandler Handler = new();

        ClaimsPrincipal Token = Handler.ValidateToken(BearerHeader.Parameter, ValidationParameters, out SecurityToken ValidatedToken);

        Dictionary<string, string> claims = Token.Claims.ToDictionary(t => t.Type, t => t.Value);

        return claims[claim];
    }

    // Password encryption
    public string EncryptPassword(User user, string password)
    {
        return string.Format($"{Environment.GetEnvironmentVariable("TokenHashBase")!}${PasswordHasher.HashPassword(user, password)}");
    }

    // Check if hash in the hashed password is supported
    public bool IsHashSupported(string hashString)
    {
        return hashString.Contains($"{Environment.GetEnvironmentVariable("TokenHashBase")!}");
    }

    // Verify the password
    public async Task<bool> VerifyPassword(User user, string storedPassword, string password)
    {
        // Check the hash
        if (!IsHashSupported(storedPassword)) {
            throw new AuthenticationException("The password hashtype is not supported");
        }

        // Extract the iteration and Base64 hash string from the hashed password string
        string[] splittedHashString = storedPassword.Replace($"{Environment.GetEnvironmentVariable("TokenHashBase")!}", "").Split('$');
        string hashedPassword = splittedHashString[1];

        PasswordVerificationResult result = PasswordHasher.VerifyHashedPassword(user, hashedPassword, password);

        switch (result) {
            case (PasswordVerificationResult)0:
                return false;
            case (PasswordVerificationResult)1:
                return true;
            case (PasswordVerificationResult)2:
                user.Password = EncryptPassword(user, password);
                await UserRepository.SaveChanges();
                return true;
        }

        return false;
    }
}
