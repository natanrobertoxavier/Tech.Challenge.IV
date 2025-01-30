namespace Tech.Challenge.Persistence.Domain.Repositories.Contact;
public interface IContactDeleteOnlyRepository
{
    void Remove(Entities.Contact contact);
}
