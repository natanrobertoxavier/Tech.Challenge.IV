using TokenService.Manager.Controller;

namespace User.Query.Integration.Tests.Fakes.Entities;
public class UserBuilder
{
    public static (Domain.Entities.User user, string password) Build()
    {
        var password = "123456";
        var id = Guid.NewGuid();

        var encryptor = new PasswordEncryptor("%xIQ*83Y0K!@");

        var builtUser = new Domain.Entities.User()
        {
            Id = id,
            Name = "John Cena",
            Email = $"{id.ToString()[..6]}@email.com",
            Password = encryptor.Encrypt(password)
        };

        return (builtUser, password);
    }
}
