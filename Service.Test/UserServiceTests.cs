using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Moq;
using Repository.Interfaces;
using Service.Exceptions;
using Xunit;

namespace Service.Test;
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly Mock<IGroupRepository> _mockGroupRepository;
    private readonly Mock<ISetRepository> _mockSetRepository;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _mockRepository = new();
        _mockGroupRepository = new();
        _mockSetRepository = new();
        _service = new UserService(new LoggerFactory(), _mockRepository.Object, _mockGroupRepository.Object, _mockSetRepository.Object);
    }

    [Fact]
    public async Task Get_All_Users_Should_Return_An_Array_Of_Users()
    {
        User[] mockUsers = new[] {
            new User("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "HFr33zing#1!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true),
            new User("2", "mruisberg@mail.com", "Marjan", "Ruisberg", "Mjanneke34", "MJ2U#2", UserRole.Teacher, DateTime.UtcNow, null!, "MLDK-PACL-WUDB-LZQW", true),
        };

        _mockRepository.Setup(u => u.GetAllAsync()).Returns(mockUsers.ToAsyncEnumerable());

        ICollection<User> users = await _service.GetUsers();

        Assert.Equal(2, users.Count);
    }

    [Fact]
    public void Get_All_Users_Should_Throw_Not_Found_Exception()
    {
        _mockRepository.Setup(u => u.GetAllAsync()).Returns(() => null);

        Assert.ThrowsAsync<NotFoundException>(async () => await _service.GetUsers());
    }

    [Fact]
    public async Task Get_By_User_Id_Should_Return_The_User_With_Given_Id()
    {
        _mockRepository.Setup(u => u.GetByIdAsync("1")).ReturnsAsync(() => new User("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "HFr33zing#1!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true));
        User user = await _service.GetUserById("1");

        Assert.Equal("1", user.Id);
    }

    [Fact]
    public void Get_By_User_Id_Should_Throw_Not_Found_Exception()
    {
        _mockRepository.Setup(u => u.GetByIdAsync(It.IsNotIn("1"))).ReturnsAsync(() => null);

        Assert.ThrowsAsync<NotFoundException>(() => _service.GetUserById("naN"));
    }

    [Fact]
    public async Task Create_User_Should_Return_A_User_With_User_Id()
    {
        _mockRepository.Setup(u => u.InsertAsync(It.IsAny<User>())).Verifiable();
        _mockRepository.Setup(u => u.SaveChanges()).Verifiable();

        User newUser = new();
        User user = await _service.CreateUser(newUser);

        Assert.NotNull(user.Id);

        _mockRepository.Verify(u => u.InsertAsync(It.IsAny<User>()), Times.Once);
        _mockRepository.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Update_User_Should_Have_Properties_Changed()
    {
        User user = new("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "HFr33zing#1!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true);

        _mockRepository.Setup(u => u.GetByIdAsync("1")).ReturnsAsync(() => user);
        _mockRepository.Setup(u => u.SaveChanges()).Verifiable();

        UpdateUserDTO changes = new() { Email = "testemail@mail.com", FirstName = "Harry", LastName = "de Groot", UserName = "HarryG#205!", Password = "H4rr1Dman#28", Role = UserRole.Administrator };
        await _service.UpdateUser("1", changes);

        Assert.Equal("testemail@mail.com", user.Email);
        Assert.Equal("Harry", user.FirstName);
        Assert.Equal("de Groot", user.LastName);
        Assert.Equal("HarryG#205!", user.UserName);
        Assert.Equal("H4rr1Dman#28", user.Password);
        Assert.Equal(UserRole.Administrator, user.Role);

        _mockRepository.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Update_User_Should_Throw_Not_Found_Exception()
    {
        _mockRepository.Setup(u => u.GetByIdAsync(It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("user"));

        UpdateUserDTO changes = new() { Email = "testemail@mail.com", FirstName = "Harry", LastName = "de Groot", UserName = "HarryG#205!", Password = "H4rr1Dman#28", Role = UserRole.Administrator };

        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateUser("1", changes));
    }

    [Fact]
    public async Task Delete_User_Should_Change_Is_Active_Status_As_Soft_Deletion()
    {
        User user = new("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "HFr33zing#1!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true);

        _mockRepository.Setup(u => u.GetByIdAsync("1")).ReturnsAsync(() => user);
        _mockRepository.Setup(u => u.SaveChanges()).Verifiable();

        await _service.DeleteUser("1");

        Assert.False(user.IsActive);

        _mockRepository.Verify(u => u.SaveChanges(), Times.Once);
    }

    [Fact]
    public void Delete_User_Should_Throw_Not_Found_Exception()
    {
        _mockRepository.Setup(u => u.GetByIdAsync(It.IsNotIn("1"))).ReturnsAsync(() => null);

        Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteUser("naN"));
    }

    [Fact]
    public async Task Get_User_Groups_Should_Return_An_Array_Of_Groups_The_User_Is_In()
    {
        User[] mockUsers = new[] {
            new User("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "HFr33zing#1!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true),
            new User("2", "mruisberg@mail.com", "Marjan", "Ruisberg", "Mjanneke34", "MJ2U#2", UserRole.Teacher, DateTime.UtcNow, null!, "MLDK-PACL-WUDB-LZQW", true),
        };

        _mockRepository.Setup(u => u.GetAllAsync()).Returns(() => mockUsers.ToAsyncEnumerable());
        _mockRepository.Setup(u => u.SaveChanges()).Verifiable();

        Group[] mockGroups = new[] {
            new Group("1", "bestgroup", "this is our group", EducationType.Mavo, SchoolYear.One, null!, mockUsers),
            new Group("2", "second bestgroup", "this is the rest of the groups", EducationType.Havo, SchoolYear.One, null!, null!),
        };

        _mockGroupRepository.Setup(g => g.Include(g => g.Users.Where(u => u.Id == "1")).GetAllAsync()).Returns(mockGroups.ToAsyncEnumerable());
        _mockGroupRepository.Setup(g => g.SaveChanges()).Verifiable();

        ICollection<Group> groups = await _service.GetUserGroups("1");

        User[] users = (User[])groups.First().Users;
        Assert.Equal("1", users[0].Id);
    }

    [Fact]
    public void Get_User_Groups_Should_Throw_Not_Found_Exception()
    {
        _mockGroupRepository.Setup(g => g.GetAllAsync()).Returns(() => null!);

        Assert.ThrowsAsync<NotFoundException>(async () => await _service.GetUserGroups("1"));
    }

    [Fact]
    public async Task Get_User_Sets_Should_Return_A_List_Of_Sets_That_Contains_The_User()
    {
        User[] mockUsers = new[] {
            new User("1", "hdevries@mail.com", "Henk", "de Vries", "HFreeze#902", "HFr33zing#1!", UserRole.Student, DateTime.UtcNow, null!, "UREI-POIQ-DMKL-ALQF", true),
            new User("2", "mruisberg@mail.com", "Marjan", "Ruisberg", "Mjanneke34", "MJ2U#2", UserRole.Teacher, DateTime.UtcNow, null!, "MLDK-PACL-WUDB-LZQW", true),
        };

        _mockRepository.Setup(u => u.GetAllAsync()).Returns(() => mockUsers.ToAsyncEnumerable());
        _mockRepository.Setup(u => u.SaveChanges()).Verifiable();

        Set[] mockSets = new[] {
            new Set("1", "Set 1", null!),
            new Set("2", "Set 2", null!) { Users = mockUsers },
        };

        _mockSetRepository.Setup(s => s.Include(s => s.Users.Where(u => u.Id == "2")).GetAllAsync()).Returns(mockSets.ToAsyncEnumerable());

        ICollection<Set> sets = await _service.GetUserSets("2");

        User[] users = (User[])sets.Last().Users;
        Assert.Equal("2", users[1].Id);

    }

    [Fact]
    public void Get_User_Sets_Should_Throw_Not_Found_Exception()
    {
        _mockSetRepository.Setup(s => s.GetAllAsync()).Returns(() => null);

        Assert.ThrowsAsync<NotFoundException>(async () => await _service.GetUserSets("1"));
    }
}
