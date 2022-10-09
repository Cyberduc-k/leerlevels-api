namespace Model.DTO;

public class UserDTO
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
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
