using System.Net;
using API.Controllers;
using API.Test.Mock;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Model.Response;
using Moq;
using Service.Exceptions;
using Xunit;

namespace API.Test;

public class UserControllerTests : ControllerTestsBase
{

    private readonly UserController _controller;

    public UserControllerTests()
    {
        _controller = new(new LoggerFactory(), _tokenService.Object, _mapper, _userService.Object);

        User user = new() { Id = "1", Email = "John@gmail.com", FirstName = "John", LastName = "Doe", UserName = "JohnD#1", Password = "J0nh#001!", Role = UserRole.Student, LastLogin = DateTime.Parse("2022-10-05 13:27:00"), LastDeviceHandle = "11", ShareCode = "AAAA-BBBB-CCCC-DDDD", IsActive = true };

        Group userGroup = new() { Id = "2", Name = "inholland", Subject = "IT", EducationType = EducationType.Havo, SchoolYear = SchoolYear.One, Users = new User[] { user } };

        Set userSet = new() { Id = "3", Users = new User[] { user } };

        _userService.Setup(s => s.GetUsers(int.MaxValue, 0)).ReturnsAsync(() => new User[] { user });
        _userService.Setup(s => s.GetUserById("1")).ReturnsAsync(() => user);
        _userService.Setup(s => s.GetUserById(It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("user"));
        _userService.Setup(s => s.CreateUser(It.IsAny<User>())).Verifiable();
        _userService.Setup(s => s.UpdateUser("1", It.IsAny<UpdateUserDTO>())).Verifiable();
        _userService.Setup(s => s.UpdateUser(It.IsNotIn("1"), It.IsAny<UpdateUserDTO>())).ThrowsAsync(new NotFoundException("user to update"));
        _userService.Setup(s => s.DeleteUser("1")).Verifiable();
        _userService.Setup(s => s.DeleteUser(It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("user to delete"));
        _userService.Setup(s => s.GetUserGroups("1")).ReturnsAsync(() => new Group[] { userGroup });
        _userService.Setup(s => s.GetUserGroups(It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("groups the user is in"));
        _userService.Setup(s => s.GetUserSets("1")).ReturnsAsync(() => new Set[] { userSet });
        _userService.Setup(s => s.GetUserSets(It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("sets the user has started on"));
    }

    [Fact]
    public async Task Get_Users_Should_Respond_OK()
    {
        /*User user = new() { Id = "1", Email = "John@gmail.com", FirstName = "John", LastName = "Doe", UserName = "JohnD#1", Password = "J0nh#001!", Role = UserRole.Teacher, LastLogin = DateTime.Parse("2022-10-05 13:27:00"), LastDeviceHandle = "11", ShareCode = "AAAA-BBBB-CCCC-DDDD", IsActive = true };

        string token = new JwtSecurityTokenHandler().WriteToken(await tokenService.CreateToken(user));

        HttpRequestData request = MockHelpers.CreateHttpRequestData(null!, token);*/

        HttpRequestData request = MockHelpers.CreateHttpRequestData();

        HttpResponseData response = await _controller.GetUsers(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_Users_Should_Respond_Json()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetUsers(request);
        Paginated<UserResponse>? result = await response.ReadFromJsonAsync<Paginated<UserResponse>>();

        Assert.Equal("application/json; charset=utf-8", GetHeaderValue(response, "Content-Type"));
        Assert.NotNull(result);
        Assert.Single(result!.Items);
    }

    [Fact]
    public async Task Create_User_Should_Respond_Ok()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData(new UserDTO() { Email = "Mary@gmail.com", FirstName = "Mary", LastName = "Sue", UserName = "MarySue#22", Password = "M4rySu3san#22!", Role = UserRole.Teacher });
        HttpResponseData response = await _controller.CreateUser(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _userService.Verify(s => s.CreateUser(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Create_User_Should_Throw_Null_Reference_Exception()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();

        await Assert.ThrowsAsync<NullReferenceException>(() => _controller.CreateUser(request));
        _userService.Verify(s => s.CreateUser(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Update_User_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData(new UpdateUserDTO() { Email = "JohnDoe@gmail.com", FirstName = "John", LastName = "Doe", UserName = "JohnD#1", Password = "J0nh#001!", Role = UserRole.Student });
        HttpResponseData response = await _controller.UpdateUser(request, "1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _userService.Verify(s => s.UpdateUser("1", It.IsAny<UpdateUserDTO>()), Times.Once);
    }

    [Fact]
    public async Task Update_User_Should_Throw_Not_Found_Exception()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData(new UpdateUserDTO() { Email = "DoesnotExist@gmail.com" });

        await Assert.ThrowsAsync<NotFoundException>(() => _controller.UpdateUser(request, "INVALID"));
    }

    [Fact]
    public async Task Delete_User_Should_Respond_Ok()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.DeleteUser(request, "1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _userService.Verify(s => s.DeleteUser("1"), Times.Once);
    }

    [Fact]
    public async Task Delete_User_Should_Throw_Not_Found_Exception()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();

        await Assert.ThrowsAsync<NotFoundException>(() => _controller.DeleteUser(request, "INVALID"));
    }

    [Fact]
    public async Task Get_User_Groups_Should_Respond_Ok()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();

        HttpResponseData response = await _controller.GetUserGroups(request, "1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_User_Groups_Should_Throw_Not_Found_Exception()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();

        await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetUserGroups(request, "INVALID"));
    }

    [Fact]
    public async Task Get_User_Sets_Should_Respond_Ok()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();

        HttpResponseData response = await _controller.GetUserSets(request, "1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_User_Sets_Should_Throw_Not_Found_Exception()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();

        await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetUserSets(request, "INVALID"));
    }
}