using Tech.Challenge.Persistence.Domain.Entities;
using Tech.Challenge.Persistence.Domain.Repositories.Region;

namespace Tech.Challenge.Persistence.Infrasctructure.RepositoryAccess.Repository.Region;
public class RegionWriteOnlyRepository(TechChallengeContext context) : IRegionWriteOnlyRepository
{
    private readonly TechChallengeContext _context = context;

    public async Task Add(RegionDDD ddd) =>
        await _context.DDDRegions.AddAsync(ddd);
}