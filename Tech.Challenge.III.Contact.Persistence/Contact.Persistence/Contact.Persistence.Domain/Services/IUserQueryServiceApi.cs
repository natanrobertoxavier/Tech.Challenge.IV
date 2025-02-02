using Contact.Persistence.Domain.ResultServices;

namespace Contact.Persistence.Domain.Services;
public interface IUserQueryServiceApi
{
    Task<Result<ThereIsUserResult>> ThereIsUserWithEmailAsync(string email);
    Task<Result<UserResult>> RecoverByEmailAsync(string email);
}
