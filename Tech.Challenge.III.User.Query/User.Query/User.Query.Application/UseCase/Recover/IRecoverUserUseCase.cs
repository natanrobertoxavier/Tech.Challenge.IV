
using User.Query.Communication.Request;
using User.Query.Communication.Response;

namespace User.Query.Application.UseCase.Recover;
public interface IRecoverUserUseCase
{
    Task<Result<ResponseExistsUserJson>> ThereIsUserWithEmailAsync(string email);
    Task<Result<ResponseUserJson>> RecoverByEmailAsync(string email);
    Task<Result<ResponseUserJson>> RecoverEmailPassword(RequestEmailPasswordUserJson request);
}
