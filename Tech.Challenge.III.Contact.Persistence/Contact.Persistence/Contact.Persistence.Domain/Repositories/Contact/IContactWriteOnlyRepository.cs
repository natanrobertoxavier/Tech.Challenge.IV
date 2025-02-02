namespace Contact.Persistence.Domain.Repositories.Contact;
public interface IContactWriteOnlyRepository
{
    void Update(Entities.Contact contact);
}
