﻿using System.IdentityModel.Tokens.Jwt;
using API.Mappings;
using AutoMapper;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Moq;
using Service.Interfaces;

namespace API.Test;

public abstract class ControllerTestsBase
{
    protected readonly IMapper _mapper;
    protected readonly Mock<IUserService> _userService;
    protected readonly Mock<ITokenService> _tokenService;

    protected User User => new User() { Id = "1", Role = UserRole.Student };

    public ControllerTestsBase()
    {
        _userService = new();
        _tokenService = new();

        ServiceProvider services = new ServiceCollection()
            .AddSingleton(_userService.Object)
            .AddSingleton(_tokenService.Object)
            .AddSingleton<UserConverter>()
            .AddSingleton<ForumConverter>()
            .AddSingleton<ForumReplyConverter>()
            .BuildServiceProvider();

        _mapper = new Mapper(new MapperConfiguration(c => {
            c.ConstructServicesUsing(s => services.GetRequiredService(s));
            c.AddProfile<MappingProfile>();
        }));

        // authenticate all requests.
        _tokenService.Setup(s => s.AuthenticationValidation(It.IsAny<HttpRequestData>())).ReturnsAsync(() => true);
        _tokenService.Setup(s => s.CreateToken(It.IsAny<User>())).ReturnsAsync(It.IsAny<JwtSecurityToken>());
        _userService.Setup(s => s.GetUserById("1")).ReturnsAsync(() => User);
    }

    protected string GetHeaderValue(HttpResponseData resp, string headerName)
    {
        return resp.Headers.First(h => h.Key == headerName).Value.First();
    }
}