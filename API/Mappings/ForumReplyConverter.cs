using AutoMapper;
using Model;
using Model.DTO;
using Service.Interfaces;

namespace API.Mappings;

public class ForumReplyConverter : ITypeConverter<ForumReplyDTO, Task<ForumReply>>
{
    private readonly IUserService _userService;

    public ForumReplyConverter(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ForumReply> Convert(ForumReplyDTO source, Task<ForumReply> destination, ResolutionContext context)
    {
        return new() {
            Text = source.Text,
            From = await _userService.GetUserById(source.FromId)
        };
    }
}
