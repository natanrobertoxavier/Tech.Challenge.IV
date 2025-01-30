namespace User.Query.Domain.Repositories;
public interface IUserReadOnlyRepository
{
    Task<bool> ThereIsUserWithEmailAsync(string email);
    Task<Entities.User> RecoverByEmailAsync(string email);
    Task<Entities.User> RecoverEmailPasswordAsync(string email, string password);
}
