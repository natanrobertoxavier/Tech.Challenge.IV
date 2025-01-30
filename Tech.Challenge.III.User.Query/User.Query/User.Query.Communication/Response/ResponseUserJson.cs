namespace User.Query.Communication.Response;

public class ResponseUserJson(
    Guid id,
    DateTime registrationDate,
    string name,
    string email,
    string password)
{
    public Guid Id { get; set; } = id;
    public DateTime RegistrationDate { get; set; } = registrationDate;
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}

