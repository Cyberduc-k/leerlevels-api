using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model;
public class User
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public UserRole userRole { get; set; }
    public DateTime LastLogin { get; set; }
    public string ShareCode { get; set; }
    public Boolean IsActive { get; set; }

    public User()
    {
    }

    public User(string id, string email, string firstName, string lastName, string userName, string password, UserRole userRole, DateTime lastLogin, string shareCode, bool isActive)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Password = password;
        this.userRole = userRole;
        LastLogin = lastLogin;
        ShareCode = shareCode;
        IsActive = isActive;
    }
}
