using RabbitMQ.Client;
using Tech.Challenge.Persistence.Api.Models;
using Tech.Challenge.Persistence.Domain.Repositories;
using Tech.Challenge.Persistence.Domain.Repositories.Contact;
using Tech.Challenge.Persistence.Exceptions;
using Tech.Challenge.Persistence.Infrasctructure.Queue;

namespace Tech.Challenge.Persistence.Api.Listeners;

public class DeleteContactListener(
    IConnectionFactory connectionFactory,
    Serilog.ILogger logger,
    IServiceScopeFactory scopeFactory)
    : QueueListenerBase<DeleteContactModel>(
        RabbitMqConstants.DeleteContactQueueName,
        connectionFactory)
{
    private readonly Serilog.ILogger _logger = logger;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    protected override async Task ProcessMessageAsync(DeleteContactModel message)
    {
        try
        {
            _logger.Information($"Starting contact deletion processing. ContactId: {message.ContactId}");

            using (var scope = _scopeFactory.CreateScope())
            {
                var workUnit = scope.ServiceProvider.GetRequiredService<IWorkUnit>();
                var contactReadOnlyRepository = scope.ServiceProvider.GetRequiredService<IContactReadOnlyRepository>();
                var contactDeleteOnlyRepository = scope.ServiceProvider.GetRequiredService<IContactDeleteOnlyRepository>();

                var contact = await contactReadOnlyRepository.RecoverByContactIdAsync(message.ContactId);

                if (contact is null)
                {
                    var notFoundMessage = $"No information found for id: {message.ContactId}";

                    throw new NotFoundException(notFoundMessage);
                }

                contactDeleteOnlyRepository.Remove(contact);
                await workUnit.Commit();
            }

            _logger.Information($"Contact deletion processing completed. ContactId: {message.ContactId}");
        }
        catch (NotFoundException ex)
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

        if (ex is NotFoundException)
        {
            _logger.Information("No contact found to delete, skipping.");
        }
        else
        {
            _logger.Fatal("Critical error occurred, manual intervention may be required.");
        }

        await Task.CompletedTask;
    }
}