using AutoMapper;
using Model;
using Model.DTO;
using Service.Interfaces;

namespace API.Mappings;
internal class UserConverter : ITypeConverter<UserDTO, User>
{
    ITokenService _tokenService;
    public UserConverter(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public User Convert(UserDTO source, User destination, ResolutionContext context)
    {
        return new() {
            Email = source.Email,
            FirstName = source.FirstName,
            LastName = source.LastName,
            UserName = source.UserName,
            Password = _tokenService.EncryptPassword(source.Password),
            Role = source.Role,
            //2do: check or create method to auto-generate the proper format of a shareCode for a new user (sharecode is meant for coaching purposes btw)
            ShareCode = Guid.NewGuid().ToString()

        };
    }
}
