using Microsoft.EntityFrameworkCore;
using User.Query.Domain.Repositories;

namespace User.Query.Infrastructure.AccessRepository;
public class UserRepository(
    TechChallengeContext context) : IUserReadOnlyRepository
{
    private readonly TechChallengeContext _context = context;

#pragma warning disable CS8603

    public async Task<bool> ThereIsUserWithEmailAsync(string email) =>
        await _context.Users.AnyAsync(c => c.Email.Equals(email));

    public async Task<Domain.Entities.User> RecoverByEmailAsync(string email) =>
        await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email.Equals(email));

    public async Task<Domain.Entities.User> RecoverEmailPasswordAsync(string email, string password) =>
        await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email.Equals(email) &&
                                 c.Password.Equals(password));
}
