using Contact.Persistence.Application.Services.LoggedUser;
using Contact.Persistence.Communication.Response;
using Contact.Persistence.Domain.Messages.DomaiEvents;
using Contact.Persistence.Domain.ResultServices;
using Contact.Persistence.Domain.Services;
using MediatR;
using Serilog;
using TokenService.Manager.Controller;

namespace Contact.Persistence.Application.UseCase.Contact.Delete;
public class DeleteContactUseCase(
    IContactQueryServiceApi contactQueryServiceApi,
    IMediator mediator,
    ILoggedUser loggedUser,
    TokenController tokenController,
    ILogger logger) : IDeleteContactUseCase
{
    private readonly IContactQueryServiceApi _contactQueryServiceApi = contactQueryServiceApi;
    private readonly IMediator _mediator = mediator;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly TokenController _tokenController = tokenController;
    private readonly ILogger _logger = logger;

    public async Task<Communication.Response.Result<MessageResult>> DeleteContactAsync(Guid id)
    {
        var output = new Communication.Response.Result<MessageResult>();

        try
        {
            _logger.Information($"Start {nameof(DeleteContactAsync)}.");

            var loggedUser = await _loggedUser.RecoverUser();
            var token = _tokenController.GenerateToken(loggedUser.Email);

            var contact = await _contactQueryServiceApi.RecoverContactByIdAsync(id, token);

            var errorMessage = GetErrorMessageIfContactFails(contact);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _logger.Information($"{nameof(DeleteContactAsync)} - {errorMessage}");
                return output.Failure(new List<string> { errorMessage });
            }

            await _mediator.Publish(new DeleteContactDomainEvent(id));

            _logger.Information($"End {nameof(DeleteContactAsync)}.");

            return output.Success(new MessageResult("Deleção em processamento."));
        }
        catch (Exception ex)
        {
            var errorMessage = $"There is an error: {ex.Message}";
            _logger.Error(ex, errorMessage);
            return output.Failure(new List<string> { errorMessage });
        }
    }

    private static string GetErrorMessageIfContactFails(Domain.ResultServices.Result<ContactResult> contact)
    {
        var @return = string.Empty;

        if (!contact.IsSuccess || contact.Data is null)
        {
            var error = string.IsNullOrEmpty(contact.Error) ? "Não foram encontradas informações para o contato informado." : contact.Error;

            @return = $"An error occurred when calling the Region.Query Api. Error: {error}";
        }

        return @return;
    }
}
