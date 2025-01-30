namespace Tech.Challenge.Persistence.Domain.Repositories.Contact;
public interface IContactWriteOnlyRepository
{
    Task Add(Entities.Contact contact);
}
