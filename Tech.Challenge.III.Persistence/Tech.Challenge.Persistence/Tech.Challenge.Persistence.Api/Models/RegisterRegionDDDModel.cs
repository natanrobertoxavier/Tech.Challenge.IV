namespace Tech.Challenge.Persistence.Api.Models;

public class RegisterRegionDDDModel
{
    public Guid Id { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int DDD { get; set; }
    public string Region { get; set; }
    public Guid UserId { get; set; }
}
