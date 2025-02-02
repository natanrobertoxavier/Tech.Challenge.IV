
using Contact.Persistence.Domain.ResultServices;

namespace Contact.Persistence.Domain.Services;
public interface IRegionQueryServiceApi
{
    Task<Result<RegionDDDResult>> RecoverByDDDAsync(int dDD, string token);
}
