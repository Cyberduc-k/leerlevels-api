using AutoMapper;
using Model;
using Model.DTO;
using Model.Response;

namespace API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Forum, ForumResponse>();
        CreateMap<ForumDTO, Task<Forum>>().ConvertUsing<ForumConverter>();
        CreateMap<ForumReply, ForumReplyResponse>();
        CreateMap<ForumReplyDTO, Task<ForumReply>>().ConvertUsing<ForumReplyConverter>();
        CreateMap<TargetDTO, Target>();
        CreateMap<Target, TargetResponse>();
        CreateMap<Target, Task<TargetResponse>>().ConvertUsing<BookmarkedTargetConverter>();
        CreateMap<McqDTO, Mcq>();
        CreateMap<Mcq, McqResponse>();
        CreateMap<Mcq, Task<McqResponse>>().ConvertUsing<BookmarkedMcqConverter>();
        CreateMap<AnswerOptionDTO, AnswerOption>();
        CreateMap<SetDTO, Set>();
        CreateMap<Set, SetResponse>();
        CreateMap<User, UserResponse>();
        CreateMap<GroupDTO, Group>();
        CreateMap<Group, GroupResponse>();
        CreateMap<UserDTO, User>().ConvertUsing<UserConverter>();
        CreateMap<Group, AddGroupToUserResponse>();
    }
}
