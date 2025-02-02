using Region.Query.Domain.ResultServices;

namespace Region.Query.Domain.Services;
public interface IUserQueryServiceApi
{
    Task<Result<UserResult>> RecoverByEmailAsync(string email);
}
