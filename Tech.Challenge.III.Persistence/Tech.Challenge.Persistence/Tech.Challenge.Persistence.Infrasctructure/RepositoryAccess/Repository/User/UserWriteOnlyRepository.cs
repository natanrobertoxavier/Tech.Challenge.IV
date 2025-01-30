using Tech.Challenge.Persistence.Domain.Repositories.User;

namespace Tech.Challenge.Persistence.Infrasctructure.RepositoryAccess.Repository.User;
public class UserWriteOnlyRepository(TechChallengeContext context) : IUserWriteOnlyRepository
{
    private readonly TechChallengeContext _context = context;

    public async Task Add(Domain.Entities.User user) =>
        await _context.Users.AddAsync(user);
}
