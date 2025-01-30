using Tech.Challenge.Persistence.Domain.Repositories.Contact;

namespace Tech.Challenge.Persistence.Infrasctructure.RepositoryAccess.Repository.Contact;
public class ContactDeleteOnlyRepository(TechChallengeContext context) : IContactDeleteOnlyRepository
{
    private readonly TechChallengeContext _context = context;

    public void Remove(Domain.Entities.Contact contact) =>
        _context.Contacts.Remove(contact);
}
