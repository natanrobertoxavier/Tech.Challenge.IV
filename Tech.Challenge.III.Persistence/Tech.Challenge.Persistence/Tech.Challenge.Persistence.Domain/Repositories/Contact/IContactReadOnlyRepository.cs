namespace Tech.Challenge.Persistence.Domain.Repositories.Contact;
public interface IContactReadOnlyRepository
{
    Task<Entities.Contact> RecoverByContactIdAsync(Guid id);
    Task<bool> ThereIsRegisteredContact(Guid dddId, string phoneNumber);
}
