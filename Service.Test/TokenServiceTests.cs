using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Claims;
using API.Test.Mock;
using Azure.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Model;
using Model.DTO;
using Model.Response;
using Moq;
using Newtonsoft.Json.Linq;
using Repository.Interfaces;
using Service.Exceptions;
using Xunit;
using YamlDotNet.Core.Tokens;

namespace Service.Test;
public class TokenServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly TokenService _tokenService;

    public TokenServiceTests()
    {
        Environment.SetEnvironmentVariable("LeerLevelsTokenKey", "randomstring#123455566834737!");
        Environment.SetEnvironmentVariable("TokenHashBase", "$TESTP4SSB4S3$");
        _mockUserRepository = new();
        _tokenService = new TokenService(new LoggerFactory(), _mockUserRepository.Object);
    }

    [Fact]
    public async Task Login_Should_Return_Response_With_Valid_Jwt_Security_Token()
    {

        LoginDTO login = new() { Email = "hdevries@mail.com", Password = "M4rySu3san#22!" };

        _mockUserRepository.Setup(u => u.GetByAsync(u => u.Email == login.Email)).ReturnsAsync(() => new("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "$TESTP4SSB4S3$10000$ZR9AMoHqh69WDC8SbEqMFwl2ERkrSDc62BFdt38Sx1tRaE5h", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true));
        _mockUserRepository.Setup(u => u.SaveChanges()).Verifiable();

        LoginResponse tokenResponse = await _tokenService.Login(login);

        Assert.NotNull(tokenResponse);

    }

    [Fact]
    public async Task Login_Should_Throw_Not_Found_Exception()
    {
        _mockUserRepository.Setup(u => u.GetByAsync(u => u.Email == "noemail@mail.com")).ReturnsAsync(() => null);
        Assert.ThrowsAsync<NotFoundException>(async () => await _tokenService.Login(new LoginDTO("NaN", "NaN")));
    }

    [Fact]
    public async Task Login_Should_Throw_Authentication_Exception()
    {

        // setup users

        User[] mockUsers = new[] {
            new User("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "HFr33zing#1!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true),
            new User("2", "mruisberg@mail.com", "Marjan", "Ruisberg", "Mjanneke34", "MJ2U#2", UserRole.Teacher, DateTime.UtcNow, null!, "MLDK-PACL-WUDB-LZQW", true),
        };

        _mockUserRepository.Setup(u => u.GetAllAsync()).Returns(() => mockUsers.ToAsyncEnumerable());
        _mockUserRepository.Setup(u => u.SaveChanges()).Verifiable();
        
        //assert the exception will be thrown when real email but slightly off/wrong password is used

        Assert.ThrowsAsync<AuthenticationException>(async () => await _tokenService.Login(new LoginDTO("mruisberg@mail.com", "MJ2U#1")));

    }

    [Fact]
    public async Task Create_Token_Should_Return_Valid_Jwt_Token()
    {
        User mockUser = new("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "HFr33zing#1!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true);

        JwtSecurityToken token = await _tokenService.CreateToken(mockUser);

        Dictionary<string, string> claims = token.Claims.ToDictionary(t => t.Type, t => t.Value);

        Assert.NotNull(token);

        // assert token user related claims content (if it matches the setup user of course)

        Assert.Equal("1", claims["userId"]);
        Assert.Equal("HFreeze#902", claims["userName"]);
        Assert.Equal("hdevries@mail.com", claims["userEmail"]);
        Assert.Equal(UserRole.Student.ToString(), claims["userRole"]);

        // assert other token claims
        Assert.Equal("LeerLevels", claims["iss"]);
        Assert.Equal("Users of the LeerLevels applications", claims["aud"]);

        DateTime notValidBefore = DateTimeOffset.FromUnixTimeSeconds(long.Parse(claims["nbf"])).UtcDateTime;
        DateTime expiresAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(claims["exp"])).UtcDateTime;
        DateTime issuedAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(claims["iat"])).UtcDateTime;

        Assert.True(issuedAt < DateTime.UtcNow);
        Assert.True(notValidBefore < DateTime.UtcNow);
        Assert.True(expiresAt > DateTime.UtcNow);

    }

    [Fact]
    public async Task Get_By_Value_Should_Return_Valid_ClaimsPrincipal_Value()
    {
        User mockUser = new("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "HFr33zing#1!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true);

        string token = new JwtSecurityTokenHandler().WriteToken(await _tokenService.CreateToken(mockUser));

        ClaimsPrincipal claimsPrincipal = await _tokenService.GetByValue(token);

        Assert.NotNull(claimsPrincipal);

        Assert.True(claimsPrincipal.Claims.Any());

    }

    //should return SecurityTokenSignatureKeyNotFoundException met deze string (key is anders dus:         
    [Fact]
    public async Task Get_By_Value_Should_Throw_Security_Token_Signature_Key_Not_Found_Exception()
    {
        string mockTokenValue = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIyIiwidXNlck5hbWUiOiJNYXJ5U3VlIzIyIiwidXNlckVtYWlsIjoiTWFyeVN1ZUBnbWFpbC5jb20iLCJ1c2VyUm9sZSI6IlRlYWNoZXIiLCJuYmYiOjE2NjcxMzU0NDksImV4cCI6MTY2NzEzOTA0OSwiaWF0IjoxNjY3MTM1NDQ5LCJpc3MiOiJMZWVyTGV2ZWxzIiwiYXVkIjoiVXNlcnMgb2YgdGhlIExlZXJMZXZlbHMgYXBwbGljYXRpb25zIn0.auCZWgfagqa5248fWdNOLAZ1RWgg7ITZpunU-rETB_0";
        Assert.ThrowsAsync<SecurityTokenSignatureKeyNotFoundException>(async () => await _tokenService.GetByValue(mockTokenValue));

    }

    [Fact]
    public async Task Get_By_Value_Should_Throw_Authentication_Exception()
    {
        Assert.ThrowsAsync<AuthenticationException>(async () => await _tokenService.GetByValue(null!));
    }

    [Fact]
    public async Task Authentication_Validation_Should_Return_True_From_Valid_Authorization_Header()
    {
        User mockUser = new("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "HFr33zing#1!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true);

        _mockUserRepository.Setup(u => u.GetByIdAsync("1")).ReturnsAsync(() => mockUser);
        _mockUserRepository.Setup(u => u.SaveChanges()).Verifiable();

        string token = new JwtSecurityTokenHandler().WriteToken(await _tokenService.CreateToken(mockUser));

        HttpRequestData request = MockHelpers.CreateHttpRequestData(null!, token);

        Assert.True(await _tokenService.AuthenticationValidation(request));
    }

    [Fact]
    public async Task Authentication_Validation_Without_Token_Should_Throw_Authentication_Exception()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData(null!, null!);

        Assert.False(await _tokenService.AuthenticationValidation(request));

        Assert.ThrowsAsync<AuthenticationException>(async () => await _tokenService.AuthenticationValidation(request));
    }

    [Fact]
    public async Task Authentication_Validation_With_Expired_Token_Should_Throw_Authentication_Exception()
    {
        // might have to use the content of the CreateToken method here instead
        string expiredToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIyIiwidXNlck5hbWUiOiJNYXJ5U3VlIzIyIiwidXNlckVtYWlsIjoiTWFyeVN1ZUBnbWFpbC5jb20iLCJ1c2VyUm9sZSI6IlRlYWNoZXIiLCJuYmYiOjE2NjcxNTI4OTYsImV4cCI6MTY2NzE1Mjg5OCwiaWF0IjoxNjY3MTUyODk2LCJpc3MiOiJMZWVyTGV2ZWxzIiwiYXVkIjoiVXNlcnMgb2YgdGhlIExlZXJMZXZlbHMgYXBwbGljYXRpb25zIn0.Cy9taWenEJohJHMwVwoPFGOFgYy9VbSEsPjzHf0RiXg";

        HttpRequestData request = MockHelpers.CreateHttpRequestData(null!, expiredToken);

        Assert.ThrowsAsync<AuthenticationException>(async () => await _tokenService.AuthenticationValidation(request));

    }

    [Fact]
    public async Task Authentication_Validation_With_Claims_Missing_Should_Throw_Authentication_Exception()
    {
        // same goes here, might have to create the token here instead of using a pre-made token as a string
        string invalidToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIyIiwibmJmIjoxNjY3MTU2Mjk3LCJleHAiOjE2NjcxNjM0OTcsImlhdCI6MTY2NzE1NjI5NywiaXNzIjoiTGVlckxldmVscyIsImF1ZCI6IlVzZXJzIG9mIHRoZSBMZWVyTGV2ZWxzIGFwcGxpY2F0aW9ucyJ9.yzz4lIdyUFLwKnrNK4TpGuDRJdQ7a3rFfPi0NjcGLDk";

        HttpRequestData request = MockHelpers.CreateHttpRequestData(null!, invalidToken);

        Assert.ThrowsAsync<AuthenticationException>(async () => await _tokenService.AuthenticationValidation(request));
    }

    [Fact]
    public async Task Authentication_Validation_With_Inactive_User_Should_Throw_Authentication_Exception()
    {
        User mockUser = new("1", "deleteduser@mail.com", "Del", "Eted", "removed", "1nth3v0id", UserRole.Administrator, DateTime.Parse("1987-10-05 06:10:15"), null!, "UREI-POIQ-DMKL-ALQF", false);

        _mockUserRepository.Setup(u => u.GetByIdAsync("1")).ReturnsAsync(() => mockUser);
        _mockUserRepository.Setup(u => u.SaveChanges()).Verifiable();

        string token = new JwtSecurityTokenHandler().WriteToken(await _tokenService.CreateToken(mockUser));

        HttpRequestData request = MockHelpers.CreateHttpRequestData(null!, token);

        Assert.False(await _tokenService.AuthenticationValidation(request));

        Assert.ThrowsAsync<AuthenticationException>(async () => await _tokenService.AuthenticationValidation(request));
    }

    [Fact]
    public async Task Encrypt_Password_Should_Return_Encrypted_Password()
    {
        string mockPassword = "m0ckp4ssw0rd#123!";

        string encryptedPassword = _tokenService.EncryptPassword(mockPassword);

        Assert.NotNull(encryptedPassword);

        Assert.True(_tokenService.VerifyPassword(mockPassword, encryptedPassword));
    }

    [Fact]
    public async Task Is_Hash_Supported_Should_Return_True()
    {
        string mockPassword = "m0ckp4ssw0rd#123!";

        string hashedPassword = _tokenService.EncryptPassword(mockPassword);

        Assert.True(_tokenService.IsHashSupported(hashedPassword));
    }

    [Fact]
    public async Task Is_Hash_Supported_Should_Throw_Authentication_Exception()
    {
        string wrongHashPassword = "C0MPL3T3LY0TH3RH4SH$10000$xoUFLA1yQKZA/wvfJ9aBNPAJbbUY65QLhOeNeUA+ASwM5GjK";

        Assert.False(_tokenService.IsHashSupported(wrongHashPassword));

        _ = Assert.ThrowsAsync<AuthenticationException>(() => { _tokenService.IsHashSupported(wrongHashPassword); return Task.CompletedTask; });
    }

    [Fact]
    public async Task Verify_Password_Should_Return_True()
    {
        string mockPassword = "m0ckp4ssw0rd#123!";

        string encryptedPassword = _tokenService.EncryptPassword(mockPassword);

        Assert.True(_tokenService.VerifyPassword(mockPassword, encryptedPassword));
    }

    [Fact]
    public async Task Verify_Password_Should_Throw_Authentication_Exception()
    {
        string mockPassword = "m0ckp4ssw0rd#123!";

        string incorrectEncryptedPassword = "xoUFLA1yQKZA/wvfJ9aBNPAJbbUY65QLhOeNeUA+ASwM5GjK";

        _ = Assert.ThrowsAsync<AuthenticationException>(() => { _tokenService.VerifyPassword(mockPassword, incorrectEncryptedPassword); return Task.CompletedTask; });
    }

}
