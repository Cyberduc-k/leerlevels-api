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
        CreateMap<Target, TargetResponse>();
        CreateMap<Mcq, McqResponse>();
        CreateMap<User, UserResponse>();
        CreateMap<UserDTO, User>().ConvertUsing<UserConverter>();
        CreateMap<TargetProgress, TargetProgressResponse>();
        CreateMap<McqProgress, McqProgressResponse>()
            .ForMember(m => m.AnswerOptionId, act => act.MapFrom(m => m.Answer != null ? m.Answer.Id : null));
    }
}
