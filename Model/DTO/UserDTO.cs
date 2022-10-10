using Newtonsoft.Json;

namespace Model.DTO;

public class UserDTO
{
    [JsonRequired]
    public string Email { get; set; }

    [JsonRequired]
    public string FirstName { get; set; }

    [JsonRequired]
    public string LastName { get; set; }

    [JsonRequired]
    public string UserName { get; set; }

    [JsonRequired]
    public string Password { get; set; }

    [JsonRequired]
    public UserRole Role { get; set; }

    public UserDTO()
    {
    }

    public UserDTO(string email, string firstName, string lastName, string userName, string password, UserRole role)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Password = password;
        Role = role;
    }
}
