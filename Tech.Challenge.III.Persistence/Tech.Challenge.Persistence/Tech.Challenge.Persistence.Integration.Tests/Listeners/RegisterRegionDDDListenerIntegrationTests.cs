using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Serilog;
using Tech.Challenge.Persistence.Api.Models;
using Tech.Challenge.Persistence.Domain.Repositories.Region;
using Tech.Challenge.Persistence.Infrasctructure.RepositoryAccess;
using Tech.Challenge.Persistence.Infrasctructure.RepositoryAccess.Repository.Region;
using Tech.Challenge.Persistence.Integration.Tests.Fixture;

namespace Tech.Challenge.Persistence.Integration.Tests.Listeners;
public class RegisterRegionDDDListenerIntegrationTests : IClassFixture<RabbitMqFixture>
{
    private readonly RabbitMqFixture _rabbitMqFixture;
    private readonly IServiceProvider _serviceProvider;
    private readonly IRegionWriteOnlyRepository _regionWriteOnlyRepository;
    private readonly IRegionDDDReadOnlyRepository _regionReadOnlyRepository;

    public RegisterRegionDDDListenerIntegrationTests(RabbitMqFixture rabbitMqFixture)
    {
        _rabbitMqFixture = rabbitMqFixture;

        var services = new ServiceCollection();

        var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        services.AddDbContext<TechChallengeContext>(options =>
        {
            options.UseInMemoryDatabase("InMemoryDbForTesting");
            options.UseInternalServiceProvider(provider);
        });

        services.AddSingleton<IConnectionFactory>(_rabbitMqFixture.ConnectionFactory);
        services.AddSingleton<Serilog.ILogger>(new LoggerConfiguration().WriteTo.Console().CreateLogger());
        //services.AddSingleton<IServiceScopeFactory, ServiceScopeFactory>();

        services.AddScoped<IRegionWriteOnlyRepository, RegionWriteOnlyRepository>();
        services.AddScoped<IRegionDDDReadOnlyRepository, RegionDDDReadOnlyRepository>();

        _serviceProvider = services.BuildServiceProvider();

        _regionWriteOnlyRepository = _serviceProvider.GetRequiredService<IRegionWriteOnlyRepository>();
        _regionReadOnlyRepository = _serviceProvider.GetRequiredService<IRegionDDDReadOnlyRepository>();
    }

    [Fact]
    public async Task Should_Process_Message_Successfully_When_DDD_Is_Not_Registered()
    {
        var message = new RegisterRegionDDDModel
        {
            Id = Guid.NewGuid(),
            DDD = 99,
            Region = "Sudeste",
            RegistrationDate = DateTime.Now,
            UserId = Guid.NewGuid(),
        };

        await _rabbitMqFixture.SendMessageAsync(message);

        var regionDDD = await _regionReadOnlyRepository.ThereIsDDDNumber(11);

        Assert.NotNull(regionDDD);
        Assert.False(regionDDD);
    }
}
