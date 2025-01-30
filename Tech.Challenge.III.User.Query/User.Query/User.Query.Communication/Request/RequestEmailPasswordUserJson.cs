namespace User.Query.Communication.Request;
public class RequestEmailPasswordUserJson(
    string email,
    string password)
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}
