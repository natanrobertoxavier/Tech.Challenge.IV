﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Region.Persistence.Infrastructure.RepositoryAccess;

namespace Region.Persistence.Integration.Tests;
public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private ServiceProvider _serviceProvider;
    private string _password;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ReplaceDatabase(services);
        });

        builder.UseEnvironment("IntegrationTests");

        builder.ConfigureAppConfiguration((_, appConfiguration) =>
        {
            appConfiguration.AddJsonFile("appsettings.IntegrationTests.json", optional: false, reloadOnChange: true);
        });
    }

    public TechChallengeContext GetContext() =>
        _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<TechChallengeContext>();

    public string RecoverPassword() => _password;

    private void ReplaceDatabase(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(DbContextOptions<TechChallengeContext>));

        services.Remove(descriptor);

        var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

        services.AddDbContext<TechChallengeContext>(options =>
        {
            options.UseInMemoryDatabase("InMemoryDbForTesting");
            options.UseInternalServiceProvider(provider);
        });

        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopeService = scope.ServiceProvider;
        var database = scopeService.GetRequiredService<TechChallengeContext>();

        database.Database.EnsureDeleted();
    }

    public class HttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; } = new DefaultHttpContext();
    }
}
