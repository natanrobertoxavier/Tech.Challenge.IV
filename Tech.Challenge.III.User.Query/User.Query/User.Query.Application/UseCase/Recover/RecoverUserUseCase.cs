using Serilog;
using User.Query.Communication.Request;
using User.Query.Communication.Response;
using User.Query.Domain.Repositories;

namespace User.Query.Application.UseCase.Recover;
public class RecoverUserUseCase(
    IUserReadOnlyRepository userReadOnlyRepository,
    ILogger logger) : IRecoverUserUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;
    private readonly ILogger _logger = logger;

    public async Task<Result<ResponseExistsUserJson>> ThereIsUserWithEmailAsync(string email)
    {
        var output = new Result<ResponseExistsUserJson>();

        try
        {
            _logger.Information($"Start {nameof(ThereIsUserWithEmailAsync)}. User: {email}.");

            var thereIsUser = await _userReadOnlyRepository.ThereIsUserWithEmailAsync(email);

            _logger.Information($"End {nameof(ThereIsUserWithEmailAsync)}. User: {email}.");

            return output.Success(new ResponseExistsUserJson(thereIsUser));
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format("There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(errorMessage);
        }
    }

    public async Task<Result<ResponseUserJson>> RecoverByEmailAsync(string email)
    {
        var output = new Result<ResponseUserJson>();

        try
        {
            _logger.Information($"Start {nameof(RecoverByEmailAsync)}. User: {email}.");

            var user = await _userReadOnlyRepository.RecoverByEmailAsync(email);

            if (user is null)
            {
                var notFoundMessage = string.Format("No user found with email: {0}", email);

                _logger.Warning(notFoundMessage);

                return output.Failure(notFoundMessage); ;
            }

            _logger.Information($"End {nameof(RecoverByEmailAsync)}. User: {email}.");

            return output.Success(new ResponseUserJson(user.Id, user.RegistrationDate, user.Name, user.Email, user.Password));
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format("There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(errorMessage); ;
        }
    }

    public async Task<Result<ResponseUserJson>> RecoverEmailPassword(RequestEmailPasswordUserJson request)
    {
        var output = new Result<ResponseUserJson>();

        try
        {
            _logger.Information($"Start {nameof(RecoverEmailPassword)}. User: {request.Email}.");

            var user = await _userReadOnlyRepository.RecoverEmailPasswordAsync(request.Email, request.Password);

            if (user is null)
            {
                var notFoundMessage = "Email ou senha inválidos.";

                _logger.Warning(notFoundMessage);

                return output.Failure(notFoundMessage); ;
            }

            _logger.Information($"End {nameof(RecoverEmailPassword)}. User: {request.Email}.");

            return output.Success(new ResponseUserJson(user.Id, user.RegistrationDate, user.Name, user.Email, user.Password));
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format("There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(errorMessage); ;
        }
    }
}
