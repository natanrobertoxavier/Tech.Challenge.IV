namespace Tech.Challenge.Persistence.Domain.Entities;
public class Contact(
    Guid id,
    DateTime registrationDate,
    string firstName,
    string lastName,
    Guid dDDId,
    string phoneNumber,
    string email,
    Guid userId) : BaseEntity(id, registrationDate)
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public Guid DDDId { get; set; } = dDDId;
    public string PhoneNumber { get; set; } = phoneNumber;
    public string Email { get; set; } = email;
    public Guid UserId { get; set; } = userId;

    public Contact() : this(
        Guid.Empty,
        DateTime.MinValue,
        string.Empty,
        string.Empty,
        Guid.Empty,
        string.Empty,
        string.Empty,
        Guid.NewGuid())
    { }
}
