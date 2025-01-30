namespace Tech.Challenge.Persistence.Domain.Entities;
public class User(
    Guid id,
    DateTime registrationDate,
    string name,
    string email,
    string password) : BaseEntity(id, registrationDate)
{
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;

    public User() : this(
        Guid.Empty,
        DateTime.MinValue,
        string.Empty,
        string.Empty,
        string.Empty)
    { }
}
