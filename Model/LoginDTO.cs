using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model;
public class LoginDTO
{
    public string userName { get; set; }
    public string Password { get; set; }   
    
    public LoginDTO()
    {

    }

    public LoginDTO(string userName, string password)
    {
        this.userName = userName;
        Password = password;
    }   
}
