using Microsoft.EntityFrameworkCore;
using Tech.Challenge.Persistence.Domain.Repositories.Region;

namespace Tech.Challenge.Persistence.Infrasctructure.RepositoryAccess.Repository.Region;
public class RegionDDDReadOnlyRepository(TechChallengeContext context) : IRegionDDDReadOnlyRepository
{
    private readonly TechChallengeContext _context = context;

    public async Task<bool> ThereIsDDDNumber(int ddd) =>
        await _context.DDDRegions.AnyAsync(c => c.DDD.Equals(ddd));
}
