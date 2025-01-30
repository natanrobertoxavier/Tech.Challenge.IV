using RabbitMQ.Client;
using Tech.Challenge.Persistence.Api.Models;
using Tech.Challenge.Persistence.Domain.Entities;
using Tech.Challenge.Persistence.Domain.Repositories;
using Tech.Challenge.Persistence.Domain.Repositories.Contact;
using Tech.Challenge.Persistence.Exceptions;
using Tech.Challenge.Persistence.Infrasctructure.Queue;

namespace Tech.Challenge.Persistence.Api.Listeners;

public class RegisterContactListener(
    IConnectionFactory connectionFactory,
    Serilog.ILogger logger,
    IServiceScopeFactory scopeFactory)
    : QueueListenerBase<RegisterContactModel>(
        RabbitMqConstants.RegisterContactQueueName,
        connectionFactory)
{
    private readonly Serilog.ILogger _logger = logger;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    protected async override Task ProcessMessageAsync(RegisterContactModel message)
    {
        try
        {
            _logger.Information($"Starting contact register processing. Phone Number: {message.PhoneNumber}");

            using (var scope = _scopeFactory.CreateScope())
            {
                var workUnit = scope.ServiceProvider.GetRequiredService<IWorkUnit>();
                var contactReadOnlyRepository = scope.ServiceProvider.GetRequiredService<IContactReadOnlyRepository>();
                var contactWriteOnlyRepository = scope.ServiceProvider.GetRequiredService<IContactWriteOnlyRepository>();

                var thereIsContact = await contactReadOnlyRepository.ThereIsRegisteredContact(message.DDDId, message.PhoneNumber);

                if (thereIsContact)
                {
                    var thereIsMessage = $"Contact already registered. Phone Number: {message.PhoneNumber}";

                    throw new AlreadyRegisteredException(thereIsMessage);
                }

                await contactWriteOnlyRepository.Add(MessageToEntity(message));
                await workUnit.Commit();
            }

            _logger.Information($"Contact register processing completed.");
        }
        catch (AlreadyRegisteredException ex)
        {
            await ProcessErrorAsync(ex);
        }
        catch (Exception ex)
        {
            await ProcessErrorAsync(ex);
        }
    }

    protected override async Task ProcessErrorAsync(Exception ex)
    {
        _logger.Error($"An error occurred while processing the message. Error: {ex.Message}");

        if (ex is AlreadyRegisteredException)
        {
            _logger.Information("Contact already registered, skipping.");
        }
        else
        {
            _logger.Fatal("Critical error occurred, manual intervention may be required.");
        }

        await Task.CompletedTask;
    }

    private static Contact MessageToEntity(RegisterContactModel message) =>
        new Contact(
            message.Id,
            message.RegistrationDate,
            message.FirstName,
            message.LastName,
            message.DDDId,
            message.PhoneNumber,
            message.Email,
            message.UserId
        );
}
