using Tech.Challenge.Persistence.Domain.Repositories.Contact;

namespace Tech.Challenge.Persistence.Infrasctructure.RepositoryAccess.Repository.Contact;
public class ContactWriteOnlyRepository(TechChallengeContext context) : IContactWriteOnlyRepository
{
    private readonly TechChallengeContext _context = context;

    public async Task Add(Domain.Entities.Contact contact) =>
        await _context.Contacts.AddAsync(contact);
}
