using AutoMapper;
using Contact.Persistence.Application.Services.LoggedUser;
using Contact.Persistence.Application.UseCase.Contact.Register;
using Contact.Persistence.Communication.Request;
using Contact.Persistence.Communication.Response;
using Contact.Persistence.Domain.Repositories;
using Contact.Persistence.Domain.Repositories.Contact;
using Contact.Persistence.Domain.Services;
using Contact.Persistence.Exceptions;
using Contact.Persistence.Exceptions.ExceptionBase;
using Serilog;
using TokenService.Manager.Controller;

namespace Contact.Persistence.Application.UseCase.Contact.Update;
public class UpdateContactUseCase(
    IContactQueryServiceApi contactQueryServiceApi,
    IRegionQueryServiceApi regionQueryServiceApi,
    IContactWriteOnlyRepository contactWriteOnlyRepository,
    IWorkUnit workUnit,
    IMapper mapper,
    ILoggedUser loggedUser,
    TokenController tokenController,
    ILogger logger) : IUpdateContactUseCase
{
    private readonly IRegionQueryServiceApi _regionQueryServiceApi = regionQueryServiceApi;
    private readonly IContactWriteOnlyRepository _contactWriteOnlyRepository = contactWriteOnlyRepository;
    private readonly IContactQueryServiceApi _contactQueryServiceApi = contactQueryServiceApi;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly TokenController _tokenController = tokenController;
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IWorkUnit _workUnit = workUnit;

    public async Task<Result<MessageResult>> UpdateContact(Guid id, RequestContactJson request)
    {
        var output = new Result<MessageResult>();

        try
        {
            _logger.Information($"Start {nameof(UpdateContact)}.");

            var loggedUser = await _loggedUser.RecoverUser();

            var token = _tokenController.GenerateToken(loggedUser.Email);

            var dddId = await Validate(request, token);

            var contactToUpdate = _mapper.Map<Domain.Entities.Contact>(request);

            contactToUpdate.Id = id;
            contactToUpdate.DDDId = dddId;
            contactToUpdate.UserId = loggedUser.Id;

            _contactWriteOnlyRepository.Update(contactToUpdate);
            await _workUnit.Commit();

            _logger.Information($"End {nameof(UpdateContact)}.");

            return output.Success(new MessageResult("Atualização de cadastro realizada com sucesso."));
        }
        catch (ValidationErrorsException ex)
        {
            var errorMessage = $"There are validations errors: {string.Concat(string.Join(", ", ex.ErrorMessages), ".")}";

            _logger.Error(ex, errorMessage);

            return output.Failure(ex.ErrorMessages);
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format("There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(new List<string>() { errorMessage });
        }
    }

    private async Task<Guid> Validate(RequestContactJson request, string token)
    {
        var validator = new RegisterContactValidator();
        var validationResult = validator.Validate(request);

        var dddApiResponse = await _regionQueryServiceApi.RecoverByDDDAsync(request.DDD, token);

        if (!dddApiResponse.IsSuccess)
        {
            var failMessage = $"An error occurred when calling the Region.Query Api. Error {dddApiResponse.Error}.";

            _logger.Information($"{nameof(Validate)} - {failMessage}");

            AddValidationError(validationResult, "ddd", failMessage);
        }

        if (dddApiResponse.Data?.Id == Guid.Empty)
        {
            _logger.Information($"{nameof(Validate)} - {ErrorsMessages.DDDNotFound}");

            AddValidationError(validationResult, "ddd", ErrorsMessages.DDDNotFound);
        }

        ValidateResult(validationResult);

        return dddApiResponse.Data.Id;
    }

    private static void AddValidationError(FluentValidation.Results.ValidationResult validationResult, string propertyName, string errorMessage)
    {
        validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(propertyName, errorMessage));
    }

    private static void ValidateResult(FluentValidation.Results.ValidationResult validationResult)
    {
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ValidationErrorsException(errorMessages);
        }
    }
}
