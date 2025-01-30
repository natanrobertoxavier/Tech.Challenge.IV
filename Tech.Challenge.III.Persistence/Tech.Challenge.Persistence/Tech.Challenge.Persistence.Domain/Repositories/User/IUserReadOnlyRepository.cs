namespace Tech.Challenge.Persistence.Domain.Repositories.User;
public interface IUserReadOnlyRepository
{
    Task<bool> ThereIsUserWithEmail(string email);
}
