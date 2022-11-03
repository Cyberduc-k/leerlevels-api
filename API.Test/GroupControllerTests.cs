using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using API.Controllers;
using API.Test.Mock;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Model;
using Model.Response;
using Moq;
using Repository.Interfaces;
using Service;
using Service.Interfaces;
using Xunit;

namespace API.Test;
public class GroupControllerTests : ControllerTestsBase
{
    private readonly Mock<IGroupService> _groupService;
    private readonly GroupController _controller;

    static List<Set> sets = new() { new Set() { Id = "1", Users = new List<User>(), Targets = new List<Target>() } };
    static List<User> users = new() { new User() { FirstName = "john",LastName = "De vris",UserName = "best java user", ShareCode = "IEJQWROQWR", LastDeviceHandle = "FDA" , Bookmarks = new List<Bookmark>(), LastLogin = DateTime.Now, Password ="easypassword", IsActive = true, Role = UserRole.Student,  Id = "1", Sets = sets, Groups = new List<Group>(), Email = "john@gmail.com"} };
    static List<Group> groups = new() { new Group() { EducationType = EducationType.Havo, Id = "1", Name = "best group", SchoolYear = SchoolYear.One, Sets = sets, Subject = "math", Users = users } };

    public GroupControllerTests()
    {
        _groupService = new();        
        _controller = new(new LoggerFactory(), _tokenService.Object, _groupService.Object, _mapper);     

        _groupService.Setup(x => x.GetAllGroupsAsync()).ReturnsAsync(groups);
        _groupService.Setup(x => x.GetGroupByIdAsync(groups[0].Id)).ReturnsAsync(groups[0]);

    }

    [Fact]
    public async Task Get_Groups_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetAllGroups(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_Groups_Async_should_return_A_Json()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetAllGroups(request);
        ICollection<GroupResponse>? result = await response.ReadFromJsonAsync<GroupResponse[]>();

        Assert.Equal("application/json; charset=utf-8", GetHeaderValue(response, "Content-Type"));
        Assert.NotNull(result);
        Assert.Equal(1, result!.Count);
    }

    [Fact]
    public async Task Get_Group_By_Id_Should_Return_A_Group()
    {       
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetGroupById(request, "1");
        Group? result = await response.ReadFromJsonAsync<Group>();

        Assert.NotNull(result);
        Assert.Equal(groups[0].Id, result?.Id);
       
    }

    [Fact]
    public async Task Get_Group_By_Id_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetGroupById(request, "1");

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }
}
