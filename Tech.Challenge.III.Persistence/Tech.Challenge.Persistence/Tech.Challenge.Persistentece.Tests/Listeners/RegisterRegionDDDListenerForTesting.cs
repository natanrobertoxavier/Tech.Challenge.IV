using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Serilog;
using Tech.Challenge.Persistence.Api.Listeners;
using Tech.Challenge.Persistence.Api.Models;

namespace Tech.Challenge.Persistentece.Tests.Listeners;
public class RegisterRegionDDDListenerForTesting(
    IConnectionFactory connectionFactory,
    ILogger logger,
    IServiceScopeFactory scopeFactory) : RegisterRegionDDDListener(connectionFactory, logger, scopeFactory)
{
    public new async Task ProcessMessageAsync(RegisterRegionDDDModel message)
    {
        await base.ProcessMessageAsync(message);
    }
}
