using Microsoft.EntityFrameworkCore;
using Tech.Challenge.Persistence.Domain.Repositories.User;

namespace Tech.Challenge.Persistence.Infrasctructure.RepositoryAccess.Repository.User;
public class UserReadOnlyRepository(TechChallengeContext context) : IUserReadOnlyRepository
{
    private readonly TechChallengeContext _context = context;

    public async Task<bool> ThereIsUserWithEmail(string email) =>
        await _context.Users.AnyAsync(c => c.Email.Equals(email));
}
