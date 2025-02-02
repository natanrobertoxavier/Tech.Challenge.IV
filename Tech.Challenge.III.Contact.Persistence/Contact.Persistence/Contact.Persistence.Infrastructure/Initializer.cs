using Contact.Persistence.Domain.Extensions;
using Contact.Persistence.Domain.Repositories;
using Contact.Persistence.Domain.Repositories.Contact;
using Contact.Persistence.Domain.Services;
using Contact.Persistence.Infrastructure.Queue;
using Contact.Persistence.Infrastructure.RepositoryAccess;
using Contact.Persistence.Infrastructure.RepositoryAccess.Repository.Contact;
using Contact.Persistence.Infrastructure.ServicesAccess;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Contact.Persistence.Infrastructure;
public static class Initializer
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configurationManager)
    {
        AddFluentMigrator(services, configurationManager);
        AddContext(services, configurationManager);
        AddServices(services);
        AddRepositories(services);
        AddRabbitMqDispatcher(services);
        RegisterServices(services, configurationManager);
        AddWorkUnit(services);
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configurationManager)
    {
        _ = bool.TryParse(configurationManager.GetSection("Settings:DatabaseInMemory").Value, out bool databaseInMemory);

        if (!databaseInMemory)
        {
            services.AddFluentMigratorCore().ConfigureRunner(c =>
                 c.AddMySql5()
                  .WithGlobalConnectionString(configurationManager.GetFullConnection())
                  .ScanIn(Assembly.Load("Contact.Persistence.Infrastructure")).For.All());
        }
    }

    private static void AddContext(IServiceCollection services, IConfiguration configurationManager)
    {
        _ = bool.TryParse(configurationManager.GetSection("Settings:DatabaseInMemory").Value, out bool databaseInMemory);

        if (!databaseInMemory)
        {
            var versaoServidor = new MySqlServerVersion(new Version(8, 0, 26));
            var connectionString = configurationManager.GetFullConnection();

            services.AddDbContext<TechChallengeContext>(dbContextoOpcoes =>
            {
                dbContextoOpcoes.UseMySql(connectionString, versaoServidor);
            });
        }
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IUserQueryServiceApi, UserQueryServiceApi>();
        services.AddScoped<IRegionQueryServiceApi, RegionQueryServiceApi>();
        services.AddScoped<IContactQueryServiceApi, ContactQueryServiceApi>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IContactWriteOnlyRepository, ContactRepository>();
    }

    private static void AddRabbitMqDispatcher(IServiceCollection services)
    {
        services.AddScoped<IRabbitMqEventsDispatcher, RabbitMqEventsDispatcher>();
    }

    private static void RegisterServices(IServiceCollection services, IConfiguration configurationManager)
    {
        services.AddHttpClient("UserQueryApi", client =>
        {
            client.BaseAddress = new Uri(configurationManager.GetSection("ServicesApiAddress:UserQueryApi").Value);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddHttpClient("RegionQueryApi", client =>
        {
            client.BaseAddress = new Uri(configurationManager.GetSection("ServicesApiAddress:RegionQueryApi").Value);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddHttpClient("ContactQueryApi", client =>
        {
            client.BaseAddress = new Uri(configurationManager.GetSection("ServicesApiAddress:ContactQueryApi").Value);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
    }

    private static void AddWorkUnit(IServiceCollection services)
    {
        services.AddScoped<IWorkUnit, WorkUnit>();
    }
}
