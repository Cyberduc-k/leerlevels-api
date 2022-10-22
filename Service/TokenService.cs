﻿using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker.Http;
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

    public User User { get; set; }

    public TokenService(IConfiguration Configuration, ILogger<TokenService> Logger, IUserRepository userRepository)
    {
        this.Logger = Logger;

        UserRepository = userRepository;

        Issuer = "LeerLevels";
        Audience = "Users of the LeerLevels applications";
        ValidityDuration = TimeSpan.FromDays(1); // 2do: configure an appropriate validity duration (2 hours and then generate refresh tokens? read from config somewhere when another login is required/so until how long refresh tokens are generated after init login)
        string Key = "The LeerLevels JWT token keys tring used for authentication and authorization purposes";

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

    public async Task<JwtSecurityToken> CreateToken(User user)
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

    public async Task<bool> ValidateAuthentication(HttpRequestData req)
    {
        // see if authorization key exists (also done in the JwtMiddleware)
        if(!req.Headers.Contains("Authorization")) {
            throw new AuthenticationException("No authorization header provided");
        }

        // get headers as dictionary
        Dictionary<string, string> headers = req.Headers.ToDictionary(h => h.Key, h => string.Join(";", h.Value));

        if (string.IsNullOrEmpty(headers["Authorization"])) {
            throw new AuthenticationException("No readable data present in provided authorization header");
        }

        AuthenticationHeaderValue BearerHeader = AuthenticationHeaderValue.Parse(headers["Authorization"]);

        // validation of token based on the validation parameters (also done in the JwtMiddleware)
        ClaimsPrincipal Token = await GetByValue(BearerHeader.Parameter!);

        if(Token == null || !Token.Claims.Any()) {
            throw new AuthenticationException("An Invalid or expired token was provided");
        }
        
        Dictionary<string, string> claims = Token.Claims.ToDictionary(t => t.Type, t => t.Value);

        // validation of token set user related claims (id, name and role)
        if (!claims.ContainsKey("userId") || !claims.ContainsKey("userName") || !claims.ContainsKey("userRole")) {
            throw new AuthenticationException("Insufficient data or invalid token provided");
        }

        // validation of token issuer and audience
        if (claims["iss"] != "LeerLevels" || claims["aud"] != "Users of the LeerLevels applications") {
            throw new AuthenticationException("Incorrect token issuer or audience provided");
        }

        // validation of token expiration? (is already done by the ValidateToken method but we might want to implement this here again?)
        /*if (claims["nbf"] != DateTime.UtcNow.ToLocalTime().Seconds) {

        }*/

        string userId = claims["userId"];
        User user = await UserRepository.GetByIdAsync(userId);

        if (!user!.IsActive/*|| user!.LoggedIn == false*/) {
            throw new AuthenticationException("Invalid token due to a logged out or deleted user");
        }

        //set the interface user to check authorization in the controller endpoints
        User = user;
        

        return true;
    }
}
