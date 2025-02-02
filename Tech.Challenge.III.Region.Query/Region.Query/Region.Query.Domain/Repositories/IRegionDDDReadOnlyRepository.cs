using Region.Query.Domain.Entities;

namespace Region.Query.Domain.Repositories;
public interface IRegionDDDReadOnlyRepository
{
    Task<bool> ThereIsDDDNumber(int ddd);
    Task<IEnumerable<RegionDDD>> RecoverAllAsync();
    Task<IEnumerable<RegionDDD>> RecoverListDDDByRegionAsync(string region);
    Task<RegionDDD> RecoverByDDDAsync(int dDD);
    Task<RegionDDD> RecoverByIdAsync(Guid id);
}
