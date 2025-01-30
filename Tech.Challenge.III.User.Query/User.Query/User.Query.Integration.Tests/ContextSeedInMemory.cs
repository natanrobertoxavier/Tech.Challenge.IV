using User.Query.Infrastructure.AccessRepository;
using User.Query.Integration.Tests.Fakes.Entities;

namespace User.Query.Integration.Tests;
public class ContextSeedInMemory
{
    public static (Domain.Entities.User user, string password) Seed(TechChallengeContext context)
    {
        (var user, string password) = UserBuilder.Build();

        context.Users.Add(user);
        context.SaveChanges();

        return (user, password);
    }
}
