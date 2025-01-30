namespace Tech.Challenge.Persistence.Api.Models;

public class RegisterContactModel
{
    public Guid Id { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid DDDId { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public Guid UserId { get; set; }
}
