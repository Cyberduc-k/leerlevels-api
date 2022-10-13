using AutoMapper;
using Model;
using Model.DTO;
using Service.Interfaces;

namespace API.Mappings;
internal class UserConverter : ITypeConverter<UserDTO, User>
{

    public UserConverter()
    {
    }

    public User Convert(UserDTO source, User destination, ResolutionContext context)
    {
        return new() {
            Email = source.Email,
            FirstName = source.FirstName,
            LastName = source.LastName,
            UserName = source.UserName,
            Password = source.Password,
            Role = source.Role,
            ShareCode = Guid.NewGuid().ToString()
            //check or create method to auto-generate the shareCode for a new user
        };
    }
}
