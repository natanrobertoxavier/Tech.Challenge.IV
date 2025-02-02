using AutoMapper;
using Region.Query.Communication;
using Region.Query.Communication.Response.Enum;

namespace Region.Query.Application.Services.Automapper;
public class TechChallengeProfile : Profile
{
    public TechChallengeProfile()
    {
        EntityToResponse();
    }

    private void EntityToResponse()
    {
        CreateMap<Domain.Entities.RegionDDD, Communication.Response.ResponseRegionDDDJson>()
            .ForMember(destiny => destiny.Region, config => config.MapFrom(origin => EnumExtensions.GetEnumValueFromDescription<RegionResponseEnum>(origin.Region)));
    }
}
