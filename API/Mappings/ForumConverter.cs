using AutoMapper;
using Model;
using Model.DTO;
using Service.Interfaces;

namespace API.Mappings;

internal class ForumConverter : ITypeConverter<ForumDTO, Task<Forum>>
{
    private readonly IUserService _userService;

    public ForumConverter(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Forum> Convert(ForumDTO source, Task<Forum> destination, ResolutionContext context)
    {
        return new() {
            From = await _userService.GetUserById(source.FromId),
            Title = source.Title,
            Description = source.Description,
        };
    }
}
