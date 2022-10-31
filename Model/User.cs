using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model;

public class User
{
    [OpenApiProperty(Default = "06698004-438a-46e8-b81b-ef3507a4abe5", Description = "The id of a user", Nullable = false)]
    public string Id { get; set; }

    [OpenApiProperty(Default = "email@mail.com", Description = "The email address of a user", Nullable = false)]
    public string Email { get; set; }

    [OpenApiProperty(Default = "John", Description = "The first name of a user", Nullable = false)]
    public string FirstName { get; set; }

    [OpenApiProperty(Default = "Doe", Description = "The last name of a user", Nullable = false)]
    public string LastName { get; set; }

    [OpenApiProperty(Default = "JohnnyD", Description = "The username for a user", Nullable = false)]
    public string UserName { get; set; }

    [OpenApiProperty(Default = "J0hnny#123!", Description = "The password of a user", Nullable = false)]
    public string Password { get; set; }

    [OpenApiProperty(Default = UserRole.Student, Description = "The assigned role of a user", Nullable = false)]
    public UserRole Role { get; set; }

    [OpenApiProperty(Default = "2022-10-05 13:27:00", Description = "The last login date of a user", Nullable = false)]
    public DateTime LastLogin { get; set; }

    [OpenApiProperty(Default = "681F6BB012C62A61AA2185A676B23907A5FEFE9268283DD226B08B5F0336A552", Description = "The last mobile device handle of a user", Nullable = true)]
    public string? LastDeviceHandle { get; set; }

    [OpenApiProperty(Default = "AAAA-BBBB-CCCC-DDDD", Description = "The share code of a user (coaching)", Nullable = false)]
    public string ShareCode { get; set; }

    [OpenApiProperty(Default = true, Description = "The active status of a user (false when deleted)", Nullable = false)]
    public bool IsActive { get; set; }

    public virtual ICollection<Bookmark> Bookmarks { get; set; }
    public virtual ICollection<Group> Groups { get; set; }
    public virtual ICollection<Set> Sets { get; set; }

    public User()
    {
    }

    public User(string id, string email, string firstName, string lastName, string userName, string password, UserRole role, DateTime lastLogin, string? lastDeviceHandle, string shareCode, bool isActive)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Password = password;
        Role = role;
        LastLogin = lastLogin;
        LastDeviceHandle = lastDeviceHandle;
        ShareCode = shareCode;
        IsActive = isActive;
    }
}
