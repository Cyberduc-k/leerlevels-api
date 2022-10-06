using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model;
public class UserDTO
{
    public string email { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }     
    public string userName { get; set; }     
    public string Password { get; set; }    
    public UserRole userRole { get; set; }

    public UserDTO() 
    {
    
    }

    public UserDTO(string email, string firstname, string lastname, string username, string password, UserRole userole)
    {
        this.email = email;
        this.firstName = firstname;
        this.lastName = lastname;
        this.userName = username;
        this.Password = password;
        this.userRole = userRole;

    }
}
