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
        Environment.SetEnvironmentVariable("TokenHashBase", "RandomTestPasswordHashBaseGo");
        _mockUserRepository = new();
        _tokenService = new TokenService(new LoggerFactory(), _mockUserRepository.Object);
    }

    [Fact]
    public async Task Login_Should_Return_Response_With_Valid_Jwt_Security_Token()
    {

        LoginDTO login = new() { Email = "hdevries@mail.com", Password = "M4rySu3san#22!" };

        _mockUserRepository.Setup(u => u.GetByAsync(u => u.Email == login.Email)).ReturnsAsync(() => new("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "RandomTestPasswordHashBaseGo$AQAAAAEAACcQAAAAEPvUCXnvR1fic6e98jZnZqyD2GUauqKwWnEVsMu5AGbm1PggvwocdtgxW/IIfeZh8g==", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true));
        _mockUserRepository.Setup(u => u.SaveChanges()).Verifiable();

        LoginResponse tokenResponse = await _tokenService.Login(login);

        Assert.NotNull(tokenResponse);

    }

    [Fact]
    public async Task Login_Should_Throw_Not_Found_Exception()
    {
        _mockUserRepository.Setup(u => u.GetByAsync(u => u.Email == "noemail@mail.com")).ReturnsAsync(() => null);
        await Assert.ThrowsAsync<NotFoundException>(async () => await _tokenService.Login(new LoginDTO("NaN", "NaN")));
    }

    [Fact]
    public async Task Login_Should_Throw_Authentication_Exception()
    {

        // stup login dto and return user
        LoginDTO loginDTO = new LoginDTO("mruisberg@mail.com", "MJ2U#1");

        _mockUserRepository.Setup(u => u.GetByAsync(u => u.Email == loginDTO.Email)).ReturnsAsync(() => new("1", "mruisberg@mail.com", "Marjan", "Ruisberg", "Mjanneke34", "MJ2U#2", UserRole.Teacher, DateTime.UtcNow, null!, "MLDK-PACL-WUDB-LZQW", true));
        _mockUserRepository.Setup(u => u.SaveChanges()).Verifiable();

        //assert the exception will be thrown when real email but slightly off/wrong password is used

        await Assert.ThrowsAsync<AuthenticationException>(async () => await _tokenService.Login(loginDTO));

    }

    [Fact]
    public async Task Create_Token_Should_Return_Valid_Jwt_Token()
    {
        User mockUser = new("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "HFr33zing#1!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true);

        JwtSecurityToken token = await _tokenService.CreateToken(mockUser, "no", DateTime.UtcNow.ToString());

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

        string token = new JwtSecurityTokenHandler().WriteToken(await _tokenService.CreateToken(mockUser, "no", DateTime.UtcNow.ToString()));

        ClaimsPrincipal claimsPrincipal = await _tokenService.GetByValue(token);

        Assert.NotNull(claimsPrincipal);

        Assert.True(claimsPrincipal.Claims.Any());

    }

    //should return SecurityTokenSignatureKeyNotFoundException   (change this later to create an invalid token with another invalid security signature)     
    /*[Fact]
    public async Task Get_By_Value_Should_Throw_Security_Token_Signature_Key_Not_Found_Exception()
    {
        string mockTokenValue = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIyIiwidXNlck5hbWUiOiJNYXJ5U3VlIzIyIiwidXNlckVtYWlsIjoiTWFyeVN1ZUBnbWFpbC5jb20iLCJ1c2VyUm9sZSI6IlRlYWNoZXIiLCJuYmYiOjE2NjcxMzU0NDksImV4cCI6MTY2NzEzOTA0OSwiaWF0IjoxNjY3MTM1NDQ5LCJpc3MiOiJMZWVyTGV2ZWxzIiwiYXVkIjoiVXNlcnMgb2YgdGhlIExlZXJMZXZlbHMgYXBwbGljYXRpb25zIn0.auCZWgfagqa5248fWdNOLAZ1RWgg7ITZpunU-rETB_0";
        await Assert.ThrowsAsync<SecurityTokenSignatureKeyNotFoundException>(async () => await _tokenService.GetByValue(mockTokenValue));

    }*/

    [Fact]
    public async Task Get_By_Value_Should_Throw_Authentication_Exception()
    {
        await Assert.ThrowsAsync<AuthenticationException>(async () => await _tokenService.GetByValue(null!));
    }

    [Fact]
    public async Task Authentication_Validation_Should_Return_True_From_Valid_Authorization_Header()
    {
        User mockUser = new("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "HFr33zing#1!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true);

        _mockUserRepository.Setup(u => u.GetByIdAsync("1")).ReturnsAsync(() => mockUser);
        _mockUserRepository.Setup(u => u.SaveChanges()).Verifiable();

        string token = new JwtSecurityTokenHandler().WriteToken(await _tokenService.CreateToken(mockUser, "no", DateTime.UtcNow.ToString()));

        HttpRequestData request = MockHelpers.CreateHttpRequestData(null!, token);

        Assert.True(await _tokenService.AuthenticationValidation(request));
    }

    [Fact]
    public async Task Authentication_Validation_Without_Token_Should_Return_False_And_No_Authorization_Header_Provided_Exception_Message()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData(null!, null!);

        Assert.False(await _tokenService.AuthenticationValidation(request));
    }

    [Fact]
    public async Task Authentication_Validation_With_Expired_Token_Should_Throw_Security_Token_Expired_Exception()
    {
        // might have to use the content of the CreateToken method here instead
        string expiredToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIyIiwidXNlck5hbWUiOiJNYXJ5U3VlIzIyIiwidXNlckVtYWlsIjoiTWFyeVN1ZUBnbWFpbC5jb20iLCJ1c2VyUm9sZSI6IlRlYWNoZXIiLCJuYmYiOjE2NjcxNTI4OTYsImV4cCI6MTY2NzE1Mjg5OCwiaWF0IjoxNjY3MTUyODk2LCJpc3MiOiJMZWVyTGV2ZWxzIiwiYXVkIjoiVXNlcnMgb2YgdGhlIExlZXJMZXZlbHMgYXBwbGljYXRpb25zIn0.Cy9taWenEJohJHMwVwoPFGOFgYy9VbSEsPjzHf0RiXg";

        HttpRequestData request = MockHelpers.CreateHttpRequestData(null!, expiredToken);

        await Assert.ThrowsAsync<SecurityTokenExpiredException>(async () => await _tokenService.AuthenticationValidation(request));

    }

    // (change this later to create an invalid token with missing claims instead of using a static token which doesn't work that great)
    /*[Fact]        
    public async Task Authentication_Validation_With_Claims_Missing_Should_Throw_Authentication_Exception()
    {
        // same goes here, might have to create the token here instead of using a pre-made token as a string
        string invalidToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiIyIiwibmJmIjoxNjY3MTU2Mjk3LCJleHAiOjE2NjcxNjM0OTcsImlhdCI6MTY2NzE1NjI5NywiaXNzIjoiTGVlckxldmVscyIsImF1ZCI6IlVzZXJzIG9mIHRoZSBMZWVyTGV2ZWxzIGFwcGxpY2F0aW9ucyJ9.yzz4lIdyUFLwKnrNK4TpGuDRJdQ7a3rFfPi0NjcGLDk";

        HttpRequestData request = MockHelpers.CreateHttpRequestData(null!, invalidToken);

        await Assert.ThrowsAsync<AuthenticationException>(async () => await _tokenService.AuthenticationValidation(request));
    }*/

    [Fact]
    public async Task Authentication_Validation_With_Inactive_User_Should_Return_False_And_Inactive_User_Exception_Message()
    {
        User mockUser = new("1", "deleteduser@mail.com", "Del", "Eted", "removed", "1nth3v0id", UserRole.Administrator, DateTime.Parse("1987-10-05 06:10:15"), null!, "UREI-POIQ-DMKL-ALQF", false);

        _mockUserRepository.Setup(u => u.GetByIdAsync("1")).ReturnsAsync(() => mockUser);
        _mockUserRepository.Setup(u => u.SaveChanges()).Verifiable();

        string token = new JwtSecurityTokenHandler().WriteToken(await _tokenService.CreateToken(mockUser, "no", DateTime.UtcNow.ToString()));

        HttpRequestData request = MockHelpers.CreateHttpRequestData(null!, token);

        Assert.False(await _tokenService.AuthenticationValidation(request));
    }

    [Fact]
    public async Task Encrypt_Password_Should_Return_Encrypted_Password()
    {
        User mockUser = new("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "m0ckp4ssw0rd#123!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true);

        string encryptedPassword = _tokenService.EncryptPassword(mockUser, mockUser.Password);

        Assert.NotNull(encryptedPassword);

        Assert.True(await _tokenService.VerifyPassword(mockUser, encryptedPassword, mockUser.Password));
    }

    [Fact]
    public void Is_Hash_Supported_Should_Return_True()
    {
        User mockUser = new("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "m0ckp4ssw0rd#123!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true);

        string hashedPassword = _tokenService.EncryptPassword(mockUser, mockUser.Password);

        Assert.True(_tokenService.IsHashSupported(hashedPassword));
    }

    [Fact]
    public void Is_Hash_Supported_Should_Throw_Authentication_Exception()
    {
        string wrongHashPassword = "C0MPL3T3LY0TH3RH4SH$10000$xoUFLA1yQKZA/wvfJ9aBNPAJbbUY65QLhOeNeUA+ASwM5GjK";

        Assert.False(_tokenService.IsHashSupported(wrongHashPassword));
    }

    [Fact]
    public async Task Verify_Password_Should_Return_True()
    {
        User mockUser = new("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "m0ckp4ssw0rd#123!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true);

        string encryptedPassword = _tokenService.EncryptPassword(mockUser, mockUser.Password);

        Assert.True(await _tokenService.VerifyPassword(mockUser, encryptedPassword, mockUser.Password));
    }

    [Fact]
    public async Task Verify_Password_Should_Return_False()
    {
        User mockUser = new("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "m0ckp4ssw0rd#123!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true);

        string incorrectEncryptedPassword = "RandomTestPasswordHashBaseGo$ZR9AMoHqh69WDC8SbEqMFwl2ERkrSDc62BFdt38Sx1tRaE5h";

        Assert.False(await _tokenService.VerifyPassword(mockUser, incorrectEncryptedPassword, mockUser.Password));
    }

    /*[Fact]
    public async Task Verify_Password_Should_Throw_Authentication_Exception()
    {
        User mockUser = new("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "m0ckp4ssw0rd#123!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true);

        string incorrectEncryptedPassword = "RandomIncorrectP4ssw0rdHashBase$ZR9AMoHqh69WDC8SbEqMFwl2ERkrSDc62BFdt38Sx1tRaE5h";
        Assert.Throws<AuthenticationException>(async () => { Assert.False(await _tokenService.VerifyPassword(mockUser, incorrectEncryptedPassword, mockUser.Password)); });
        //Assert.Throws<AuthenticationException>(() => { _tokenService.VerifyPassword(mockUser, incorrectEncryptedPassword, mockUser.Password);});
    }*/
}