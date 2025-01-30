namespace Tech.Challenge.Persistence.Domain.Entities;
public class BaseEntity(
    Guid id,
    DateTime registrationDate)
{
    public Guid Id { get; set; } = id;
    public DateTime RegistrationDate { get; set; } = registrationDate;

    public BaseEntity() : this(Guid.Empty, DateTime.MinValue) { }
}
