using AutoMapper;
using MirraApi.Domain.Entities;
using MirraApi.Models.RequestModels;

namespace Mirra.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserForRegistration, User>();
        CreateMap<UserForAuthentication, User>();
    }
}