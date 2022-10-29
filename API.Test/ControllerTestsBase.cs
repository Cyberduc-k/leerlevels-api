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
        _tokenService.SetupGet(s => s.User).Returns(new User() { Id = "1", Role = UserRole.Student });
        _userService.Setup(s => s.GetUserById("1")).ReturnsAsync(() => _tokenService.Object.User);
    }

    protected User User => _tokenService.Object.User;

    protected string GetHeaderValue(HttpResponseData resp, string headerName)
    {
        return resp.Headers.First(h => h.Key == headerName).Value.First();
    }
}
