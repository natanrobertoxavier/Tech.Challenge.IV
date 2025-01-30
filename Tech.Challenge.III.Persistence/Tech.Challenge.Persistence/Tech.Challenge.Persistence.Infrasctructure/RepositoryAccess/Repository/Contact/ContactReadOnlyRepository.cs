using Microsoft.EntityFrameworkCore;
using Tech.Challenge.Persistence.Domain.Repositories.Contact;

namespace Tech.Challenge.Persistence.Infrasctructure.RepositoryAccess.Repository.Contact;
public class ContactReadOnlyRepository(TechChallengeContext context) : IContactReadOnlyRepository
{
    private readonly TechChallengeContext _context = context;

    public async Task<Domain.Entities.Contact> RecoverByContactIdAsync(Guid id) =>
        await _context.Contacts.Where(c => c.Id.Equals(id)).FirstOrDefaultAsync();

    public async Task<bool> ThereIsRegisteredContact(Guid dddId, string phoneNumber) =>
        await _context.Contacts.AnyAsync(c => c.PhoneNumber.Equals(phoneNumber) &&
                                       c.DDDId.Equals(dddId));
}
