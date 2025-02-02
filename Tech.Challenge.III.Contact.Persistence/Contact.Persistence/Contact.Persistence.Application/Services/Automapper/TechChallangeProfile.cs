using AutoMapper;

namespace Contact.Persistence.Application.Services.Automapper;
public class TechChallengeProfile : Profile
{
    public TechChallengeProfile()
    {
        RequestToEntity();
    }

    private void RequestToEntity()
    {
        CreateMap<Communication.Request.RequestContactJson, Domain.Entities.Contact>()
            .ForMember(destiny => destiny.DDDId, config => config.Ignore());
    }
}
