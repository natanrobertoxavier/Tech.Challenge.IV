namespace Tech.Challenge.Persistence.Api.Models;

public class RegisterUserModel
{
    public Guid Id { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
