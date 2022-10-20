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
            //2do: check or create method to auto-generate the proper format of a shareCode for a new user
            ShareCode = Guid.NewGuid().ToString()

        };
    }
}
