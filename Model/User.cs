namespace Model;

public class User
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public DateTime LastLogin { get; set; }
    public string ShareCode { get; set; }
    public bool IsActive { get; set; }

    public User()
    {
    }

    public User(string id, string email, string firstName, string lastName, string userName, string password, UserRole role, DateTime lastLogin, string shareCode, bool isActive)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Password = password;
        Role = role;
        LastLogin = lastLogin;
        ShareCode = shareCode;
        IsActive = isActive;
    }
}
