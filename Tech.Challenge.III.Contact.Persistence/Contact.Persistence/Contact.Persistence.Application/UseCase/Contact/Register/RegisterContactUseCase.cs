using Contact.Persistence.Application.Services.LoggedUser;
using Contact.Persistence.Communication.Request;
using Contact.Persistence.Communication.Response;
using Contact.Persistence.Domain.Messages.DomaiEvents;
using Contact.Persistence.Domain.Services;
using Contact.Persistence.Exceptions;
using Contact.Persistence.Exceptions.ExceptionBase;
using MediatR;
using Serilog;
using TokenService.Manager.Controller;

namespace Contact.Persistence.Application.UseCase.Contact.Register;
public class RegisterContactUseCase(
    IContactQueryServiceApi contactQueryServiceApi,
    IRegionQueryServiceApi regionQueryServiceApi,
    IMediator mediator,
    ILoggedUser loggedUser,
    TokenController tokenController,
    ILogger logger) : IRegisterContactUseCase
{
    private readonly IContactQueryServiceApi _contactQueryServiceApi = contactQueryServiceApi;
    private readonly IRegionQueryServiceApi _regionQueryServiceApi = regionQueryServiceApi;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IMediator _mediator = mediator;
    private readonly TokenController _tokenController = tokenController;
    private readonly ILogger _logger = logger;
    private readonly Guid GuidNull = Guid.Empty;

    public async Task<Result<MessageResult>> RegisterContactAsync(RequestContactJson request)
    {
        var output = new Result<MessageResult>();

        try
        {
            _logger.Information($"Start {nameof(RegisterContactAsync)}.");

            var loggedUser = await _loggedUser.RecoverUser();

            var token = _tokenController.GenerateToken(loggedUser.Email);

            var dddId = await Validate(request, token);

            await _mediator.Publish(new ContactCreateDomainEvent(
                Guid.NewGuid(),
                request.FirstName,
                request.LastName,
                dddId,
                request.PhoneNumber,
                request.Email,
                loggedUser.Id)
            );

            _logger.Information($"End {nameof(RegisterContactAsync)}.");

            return output.Success(new MessageResult("Cadastro em processamento."));
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
        var validationResult = new RegisterContactValidator().Validate(request);

        var dddId = await ValidateDDDAsync(request.DDD, token, validationResult);
        await ValidateContactAsync(request.DDD, request.PhoneNumber, token, validationResult);

        return dddId;
    }

    private async Task<Guid> ValidateDDDAsync(int ddd, string token, FluentValidation.Results.ValidationResult validationResult)
    {
        _logger.Information($"Start {nameof(ValidateDDDAsync)}.");

        var dddApiResponse = await _regionQueryServiceApi.RecoverByDDDAsync(ddd, token);

        if (!dddApiResponse.IsSuccess)
        {
            var failMessage = $"An error occurred when calling the Region.Query Api. Error {dddApiResponse.Error}.";

            _logger.Information($"{nameof(ValidateDDDAsync)} - {failMessage}");

            AddValidationError(validationResult, "ddd", failMessage);
        }

        if (dddApiResponse.Data?.Id == Guid.Empty)
        {
            _logger.Information($"{nameof(ValidateDDDAsync)} - {ErrorsMessages.DDDNotFound}");

            AddValidationError(validationResult, "ddd", ErrorsMessages.DDDNotFound);
        }

        ValidateResult(validationResult);

        _logger.Information($"End {nameof(ValidateDDDAsync)}.");

        return dddApiResponse.Data.Id;
    }

    private async Task ValidateContactAsync(int ddd, string phoneNumber, string token, FluentValidation.Results.ValidationResult validationResult)
    {
        _logger.Information($"Start {nameof(ValidateContactAsync)}.");

        var contact = await _contactQueryServiceApi.ThereIsContactAsync(ddd, phoneNumber, token);

        if (!contact.IsSuccess)
        {
            var failMessage = $"An error occurred when calling the Contact.Query Api. Error {contact.Error}.";

            _logger.Information($"{nameof(ValidateContactAsync)} - {failMessage}");

            AddValidationError(validationResult, "contact", failMessage);
        }

        if (contact.Data.ThereIsContact)
        {
            _logger.Information($"{nameof(ValidateContactAsync)} - {ErrorsMessages.ContactAlreadyRegistered}");

            AddValidationError(validationResult, "contact", ErrorsMessages.ContactAlreadyRegistered);
        }

        _logger.Information($"End {nameof(ValidateContactAsync)}.");

        ValidateResult(validationResult);
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
