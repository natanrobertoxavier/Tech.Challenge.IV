using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tech.Challenge.Persistence.Domain.Extension;
using Tech.Challenge.Persistence.Domain.Repositories;
using Tech.Challenge.Persistence.Domain.Repositories.Contact;
using Tech.Challenge.Persistence.Domain.Repositories.Region;
using Tech.Challenge.Persistence.Domain.Repositories.User;
using Tech.Challenge.Persistence.Infrasctructure.RepositoryAccess;
using Tech.Challenge.Persistence.Infrasctructure.RepositoryAccess.Repository.Contact;
using Tech.Challenge.Persistence.Infrasctructure.RepositoryAccess.Repository.Region;
using Tech.Challenge.Persistence.Infrasctructure.RepositoryAccess.Repository.User;

namespace Tech.Challenge.Persistence.Infrasctructure;
public static class Initializer
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configurationManager)
    {
        AddFluentMigrator(services, configurationManager);
        AddContext(services, configurationManager);
        AddRepositories(services);
        AddWorkUnit(services);
    }

    private static void AddWorkUnit(IServiceCollection services)
    {
        services.AddScoped<IWorkUnit, WorkUnit>();
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configurationManager)
    {
        _ = bool.TryParse(configurationManager.GetSection("Settings:DatabaseInMemory").Value, out bool databaseInMemory);

        if (!databaseInMemory)
        {
            services.AddFluentMigratorCore().ConfigureRunner(c =>
                 c.AddMySql5()
                  .WithGlobalConnectionString(configurationManager.GetFullConnection())
                  .ScanIn(Assembly.Load("Tech.Challenge.Persistence.Infrasctructure")).For.All());
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

    private static void AddRepositories(IServiceCollection services)
    {
        services
            .AddScoped<IContactReadOnlyRepository, ContactReadOnlyRepository>()
            .AddScoped<IContactWriteOnlyRepository, ContactWriteOnlyRepository>()
            .AddScoped<IContactDeleteOnlyRepository, ContactDeleteOnlyRepository>()
            .AddScoped<IRegionDDDReadOnlyRepository, RegionDDDReadOnlyRepository>()
            .AddScoped<IRegionWriteOnlyRepository, RegionWriteOnlyRepository>()
            .AddScoped<IUserReadOnlyRepository, UserReadOnlyRepository>()
            .AddScoped<IUserWriteOnlyRepository, UserWriteOnlyRepository>();
    }
}
