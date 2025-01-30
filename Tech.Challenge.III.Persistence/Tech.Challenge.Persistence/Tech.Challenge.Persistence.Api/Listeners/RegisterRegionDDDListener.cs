using RabbitMQ.Client;
using Tech.Challenge.Persistence.Api.Models;
using Tech.Challenge.Persistence.Domain.Entities;
using Tech.Challenge.Persistence.Domain.Repositories;
using Tech.Challenge.Persistence.Domain.Repositories.Region;
using Tech.Challenge.Persistence.Exceptions;
using Tech.Challenge.Persistence.Infrasctructure.Queue;

namespace Tech.Challenge.Persistence.Api.Listeners;

public class RegisterRegionDDDListener(
    IConnectionFactory connectionFactory,
    Serilog.ILogger logger,
    IServiceScopeFactory scopeFactory)
    : QueueListenerBase<RegisterRegionDDDModel>(
        RabbitMqConstants.RegisterRegionQueueName,
        connectionFactory)
{
    private readonly Serilog.ILogger _logger = logger;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    protected async override Task ProcessMessageAsync(RegisterRegionDDDModel message)
    {
        try
        {
            _logger.Information($"Starting region DDD register processing. DDD: {message.DDD}");

            using (var scope = _scopeFactory.CreateScope())
            {
                var workUnit = scope.ServiceProvider.GetRequiredService<IWorkUnit>();
                var regionReadOnlyRepository = scope.ServiceProvider.GetRequiredService<IRegionDDDReadOnlyRepository>();
                var regionWriteOnlyRepository = scope.ServiceProvider.GetRequiredService<IRegionWriteOnlyRepository>();

                var thereIsDDD = await regionReadOnlyRepository.ThereIsDDDNumber(message.DDD);

                if (thereIsDDD)
                {
                    var thereIsMessage = $"DDD already registered. DDD: {message.DDD}";

                    throw new AlreadyRegisteredException(thereIsMessage);
                }

                await regionWriteOnlyRepository.Add(MessageToEntity(message));
                await workUnit.Commit();
            }

            _logger.Information($"Region DDD register processing completed.");
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
            _logger.Information("DDD already registered, skipping.");
        }
        else
        {
            _logger.Fatal("Critical error occurred, manual intervention may be required.");
        }

        await Task.CompletedTask;
    }

    private static RegionDDD MessageToEntity(RegisterRegionDDDModel message) =>
        new RegionDDD(
            message.Id,
            message.RegistrationDate,
            message.DDD,
            message.Region,
            message.UserId
        );
}