using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Azure.Functions.Worker.Http;
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
    private const int HashSize = 20;
    private const int SaltSize = 16;
    private const int Iterations = 10000;

    private ILogger Logger { get; }
    private IUserRepository UserRepository { get; set; }
    private string Issuer { get; }
    private string Audience { get; }
    private TimeSpan ValidityDuration { get; }
    private SigningCredentials Credentials { get; }
    private TokenIdentityValidationParameters ValidationParameters { get; }

    public User User { get; set; }

    public string Message { get; set; }

    public TokenService(ILoggerFactory loggerFactory, IUserRepository userRepository)
    {
        Logger = loggerFactory.CreateLogger<TokenService>();

        UserRepository = userRepository;

        Issuer = "LeerLevels";
        Audience = "Users of the LeerLevels applications";
        // 2do: figure out how to set up refresh tokens (send inital token and refresh token on Login, then after expiration keep providing refresh tokens until 1 year after inital expiration or Logout signal/request is received somewhere?)
        ValidityDuration = TimeSpan.FromHours(2);

        string Key = Environment.GetEnvironmentVariable("LeerLevelsTokenKey")!;

        SymmetricSecurityKey SecurityKey = new(Encoding.UTF8.GetBytes(Key));

        Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);

        ValidationParameters = new TokenIdentityValidationParameters(Issuer, Audience, SecurityKey);

    }

    public async Task<LoginResponse> Login(LoginDTO loginDTO)
    {
        //authentication of the login information
        User user = await UserRepository.GetByAsync(u => u.Email == loginDTO.Email) ?? throw new NotFoundException("user to create a valid token");

        //check if given password is valid for the saved hash of the users password
        if (!VerifyPassword(loginDTO.Password, user.Password)) {
            throw new AuthenticationException("Incorrect user password entered");
        }

        user.LastLogin = DateTime.UtcNow;

        await UserRepository.SaveChanges();

        JwtSecurityToken Token = await CreateToken(user);

        return new LoginResponse(Token);
    }

    public async Task<JwtSecurityToken> CreateToken(User user)
    {
        JwtHeader Header = new(Credentials);

        Claim[] Claims = new Claim[] {
            new Claim("userId", user.Id),
            new Claim("userName", user.UserName),
            new Claim("userEmail", user.Email),
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
            throw new AuthenticationException("No JWT type token was supplied");
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

    public async Task<bool> AuthenticationValidation(HttpRequestData req)
    {
        // check if authorization header exists (also done in the JwtMiddleware)
        if (!req.Headers.Contains("Authorization")) {
            Message = "No authorization header provided";
            return false;
        }

        // get headers as dictionary
        Dictionary<string, string> headers = req.Headers.ToDictionary(h => h.Key, h => string.Join(";", h.Value));

        if (string.IsNullOrEmpty(headers["Authorization"])) {
            Message = "No readable data present in provided authorization header";
            return false;
        }

        AuthenticationHeaderValue BearerHeader = AuthenticationHeaderValue.Parse(headers["Authorization"]);

        // validation of token based on the validation parameters (also done in the JwtMiddleware)
        ClaimsPrincipal Token = await GetByValue(BearerHeader.Parameter!);

        if (Token == null || !Token.Claims.Any()) {
            Message = "An Invalid or expired token was provided";
            return false;
        }

        Dictionary<string, string> claims = Token.Claims.ToDictionary(t => t.Type, t => t.Value);

        // validation of token set/presence of user related claims (id, name and role)
        if (!claims.ContainsKey("userId") || !claims.ContainsKey("userName") || !claims.ContainsKey("userEmail") || !claims.ContainsKey("userRole")) {
            Message = "Insufficient data or invalid token provided";
            return false;
        }

        // validation of token issuer and audience
        if (claims["iss"] != "LeerLevels" || claims["aud"] != "Users of the LeerLevels applications") {
            Message = "Incorrect token issuer or audience provided";
            return false;
        }

        // validation of the presence of token validity claims (not before, expires at & issued at)
        if (!claims.ContainsKey("nbf") || !claims.ContainsKey("exp") || !claims.ContainsKey("iat")) {
            Message = "Insufficient token expiration/validation time provided";
            return false;
        }

        // validation of token expiration (this is already done by the ValidateToken method but we might want to implement this here again, for additional safety)
        if (claims["nbf"] != null && claims["exp"] != null && claims["iat"] != null) {

            DateTime notValidBefore = DateTimeOffset.FromUnixTimeSeconds(long.Parse(claims["nbf"])).UtcDateTime;
            DateTime expiresAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(claims["exp"])).UtcDateTime;
            DateTime issuedAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(claims["iat"])).UtcDateTime;

            if (issuedAt > DateTime.UtcNow || notValidBefore > DateTime.UtcNow || expiresAt < DateTime.UtcNow) {
                Message = "Invalid or expired token detected";
                return false;
            }
        } else {
            Message = "No readable Token validation data provided";
            return false;
        }

        User user = await UserRepository.GetByIdAsync(claims["userId"]);

        if (!user!.IsActive) {
            Message = "Invalid token since this user is no longer active";
            return false;
        }

        //set the interface user to check authorization in the controller endpoints
        User = user;

        return true;
    }

    // Password encryption
    public string EncryptPassword(string password)
    {
        // Create a salt using the random number generator class
        byte[] salt;
        RandomNumberGenerator.Create().GetBytes(salt = new byte[SaltSize]);

        // Create a hash
        Rfc2898DeriveBytes pbkdf2 = new(password, salt, Iterations);
        byte[] hash = pbkdf2.GetBytes(HashSize);

        // Combine salt and hash
        byte[] hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

        // Convert to base64 string
        string base64Hash = Convert.ToBase64String(hashBytes);

        // Format hash with extra information
        return string.Format($"{Environment.GetEnvironmentVariable("TokenHashBase")!}${Iterations}${base64Hash}");

    }

    // Check if hash in the hashed password is supported
    public bool IsHashSupported(string hashString)
    {
        return hashString.Contains($"{Environment.GetEnvironmentVariable("TokenHashBase")!}");
    }

    // Verify the password
    public bool VerifyPassword(string password, string hashedPassword)
    {
        // Check the hash
        if (!IsHashSupported(hashedPassword)) {
            throw new AuthenticationException("The password hashtype is not supported");
        }

        // Extract the iteration and Base64 hash string from the hashed password string
        string[] splittedHashString = hashedPassword.Replace($"{Environment.GetEnvironmentVariable("TokenHashBase")!}", "").Split('$');
        int iterations = int.Parse(splittedHashString[1]);
        string base64Hash = splittedHashString[2];

        // Get the hash bytes
        byte[] hashBytes = Convert.FromBase64String(base64Hash);

        // Get the salt bytes
        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        // Create a hash with the given salt
        Rfc2898DeriveBytes pbkdf2 = new(password, salt, iterations);
        byte[] hash = pbkdf2.GetBytes(HashSize);

        // Work out the result
        for (int i = 0; i < HashSize; i++) {
            if (hashBytes[i + SaltSize] != hash[i]) {
                return false;
            }
        }

        return true;
    }
}
