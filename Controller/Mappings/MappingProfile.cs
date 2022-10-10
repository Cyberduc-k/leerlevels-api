using API.Mappings;
using AutoMapper;
using Model;
using Model.DTO;
using Model.Response;

namespace Controller.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Forum, ForumResponse>();
        CreateMap<ForumDTO, Task<Forum>>().ConvertUsing<ForumConverter>();
    }
}
