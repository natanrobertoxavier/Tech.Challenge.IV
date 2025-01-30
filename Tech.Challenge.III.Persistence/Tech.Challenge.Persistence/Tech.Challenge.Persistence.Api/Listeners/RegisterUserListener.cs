using RabbitMQ.Client;
using Tech.Challenge.Persistence.Api.Models;
using Tech.Challenge.Persistence.Domain.Entities;
using Tech.Challenge.Persistence.Domain.Repositories;
using Tech.Challenge.Persistence.Domain.Repositories.User;
using Tech.Challenge.Persistence.Exceptions;
using Tech.Challenge.Persistence.Infrasctructure.Queue;

namespace Tech.Challenge.Persistence.Api.Listeners;

public class RegisterUserListener(
    IConnectionFactory connectionFactory,
    Serilog.ILogger logger,
    IServiceScopeFactory scopeFactory)
    : QueueListenerBase<RegisterUserModel>(
        RabbitMqConstants.RegisterUserQueueName,
        connectionFactory)
{
    private readonly Serilog.ILogger _logger = logger;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    protected async override Task ProcessMessageAsync(RegisterUserModel message)
    {
        try
        {
            _logger.Information($"Starting user register processing. Name: {message.Name}");

            using (var scope = _scopeFactory.CreateScope())
            {
                var workUnit = scope.ServiceProvider.GetRequiredService<IWorkUnit>();
                var userReadOnlyRepository = scope.ServiceProvider.GetRequiredService<IUserReadOnlyRepository>();
                var userWriteOnlyRepository = scope.ServiceProvider.GetRequiredService<IUserWriteOnlyRepository>();

                var thereIsDDD = await userReadOnlyRepository.ThereIsUserWithEmail(message.Email);

                if (thereIsDDD)
                {
                    var thereIsMessage = $"User already registered. Name: {message.Name}";

                    throw new AlreadyRegisteredException(thereIsMessage);
                }

                await userWriteOnlyRepository.Add(MessageToEntity(message));
                await workUnit.Commit();
            }

            _logger.Information($"User register processing completed.");
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
            _logger.Information("User already registered, skipping.");
        }
        else
        {
            _logger.Fatal("Critical error occurred, manual intervention may be required.");
        }

        await Task.CompletedTask;
    }

    private static User MessageToEntity(RegisterUserModel message) =>
        new User(
            message.Id,
            message.RegistrationDate,
            message.Name,
            message.Email,
            message.Password
        );
}