using Tech.Challenge.Persistence.Domain.Entities;

namespace Tech.Challenge.Persistence.Domain.Repositories.Region;
public interface IRegionWriteOnlyRepository
{
    Task Add(RegionDDD ddd);
}
