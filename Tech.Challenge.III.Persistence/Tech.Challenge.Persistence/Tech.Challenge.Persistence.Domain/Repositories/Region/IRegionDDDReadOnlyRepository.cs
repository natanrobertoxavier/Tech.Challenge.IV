namespace Tech.Challenge.Persistence.Domain.Repositories.Region;
public interface IRegionDDDReadOnlyRepository
{
    Task<bool> ThereIsDDDNumber(int ddd);
}
