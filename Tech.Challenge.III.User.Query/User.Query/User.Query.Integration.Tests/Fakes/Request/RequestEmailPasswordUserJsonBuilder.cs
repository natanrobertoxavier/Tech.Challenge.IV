using User.Query.Communication.Request;

namespace User.Query.Integration.Tests.Fakes.Request;
public class RequestEmailPasswordUserJsonBuilder
{
    private string _email = "user@email.com";
    public string _password = "123456";

    public RequestEmailPasswordUserJson Build()
    {
        return new RequestEmailPasswordUserJson(_email, _password);
    }

    public RequestEmailPasswordUserJsonBuilder WithEmail(string value)
    {
        _email = value;
        return this;
    }

    public RequestEmailPasswordUserJsonBuilder WithPassword(string value)
    {
        _password = value;
        return this;
    }
}
