namespace User.Query.Communication.Response;
public class ResponseExistsUserJson
{
    public ResponseExistsUserJson(
        bool thereIsUser)
    {
        ThereIsUser = thereIsUser;
    }

    public bool ThereIsUser { get; set; }
}
