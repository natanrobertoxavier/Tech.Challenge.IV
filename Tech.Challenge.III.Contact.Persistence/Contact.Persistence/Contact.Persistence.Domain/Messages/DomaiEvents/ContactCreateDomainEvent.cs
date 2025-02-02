namespace Contact.Persistence.Domain.Messages.DomaiEvents;
public class ContactCreateDomainEvent(
    Guid id,
    string firstName,
    string lastName,
    Guid dDDId,
    string phoneNumber,
    string email,
    Guid userId) : DomainEvent
{
    public Guid Id { get; set; } = id;
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public Guid DDDId { get; set; } = dDDId;
    public string PhoneNumber { get; set; } = phoneNumber;
    public string Email { get; set; } = email;
    public Guid UserId { get; set; } = userId;
}
