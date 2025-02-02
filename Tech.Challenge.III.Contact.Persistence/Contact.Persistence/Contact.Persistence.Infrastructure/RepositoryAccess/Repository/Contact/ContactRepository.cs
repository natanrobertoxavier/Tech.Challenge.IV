using Contact.Persistence.Domain.Repositories.Contact;

namespace Contact.Persistence.Infrastructure.RepositoryAccess.Repository.Contact;
public class ContactRepository(
    TechChallengeContext context) : IContactWriteOnlyRepository
{
    private readonly TechChallengeContext _context = context;
    public void Update(Domain.Entities.Contact contact) =>
    _context.Contacts.Update(contact);
}
