using RabbitMq.Package.Settings;
using Serilog;
using Tech.Challenge.Persistence.Api.Listeners;
using Tech.Challenge.Persistence.Api.Models;
using Tech.Challenge.Persistence.Api.Settings;
using Tech.Challenge.Persistence.Infrasctructure.Queue;

namespace Tech.Challenge.Persistence.Api;

public static class Initializer
{
    public static void AddConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddSerilog(services);
        AddRabbitMqService(services, configuration);
        AddRabbitMqSettings(services, configuration);
    }

    private static void AddSerilog(IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        services.AddSingleton(Log.Logger);
    }

    private static void AddRabbitMqService(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));
    }

    private static void AddRabbitMqSettings(IServiceCollection services, IConfiguration configuration)
    {
        var config = new RabbitMqSettings();

        configuration.GetSection("RabbitMqSettings").Bind(config);

        services
            .AddQueueHandler(config.ComposedConnectionString)
            .DeclareQueues(
                new RabbitMqQueue(
                    exchangeName: RabbitMqConstants.ContactPersistenceExchange,
                    routingKeyName: RabbitMqConstants.RegisterContactRoutingKey,
                    queueName: RabbitMqConstants.RegisterContactQueueName),
                new RabbitMqQueue(
                    exchangeName: RabbitMqConstants.ContactPersistenceExchange,
                    routingKeyName: RabbitMqConstants.DeleteContactRoutingKey,
                    queueName: RabbitMqConstants.DeleteContactQueueName),
                new RabbitMqQueue(
                    exchangeName: RabbitMqConstants.RegionPersistenceExchange,
                    routingKeyName: RabbitMqConstants.RegisterRegionRoutingKey,
                    queueName: RabbitMqConstants.RegisterRegionQueueName),
                new RabbitMqQueue(
                    exchangeName: RabbitMqConstants.UserPersistenceExchange,
                    routingKeyName: RabbitMqConstants.RegisterUserRoutingKey,
                    queueName: RabbitMqConstants.RegisterUserQueueName)
                )
            ;

        services.AddTransient<QueueListenerBase<DeleteContactModel>, DeleteContactListener>();
        services.AddTransient<QueueListenerBase<RegisterContactModel>, RegisterContactListener>();
        services.AddTransient<QueueListenerBase<RegisterRegionDDDModel>, RegisterRegionDDDListener>();
        services.AddTransient<QueueListenerBase<RegisterUserModel>, RegisterUserListener>();

        services.AddHostedService<QueueListenerHostedService<DeleteContactModel>>();
        services.AddHostedService<QueueListenerHostedService<RegisterContactModel>>();
        services.AddHostedService<QueueListenerHostedService<RegisterRegionDDDModel>>();
        services.AddHostedService<QueueListenerHostedService<RegisterUserModel>>();
    }
}
