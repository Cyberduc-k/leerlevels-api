using API.Mappings;
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
    }
}
