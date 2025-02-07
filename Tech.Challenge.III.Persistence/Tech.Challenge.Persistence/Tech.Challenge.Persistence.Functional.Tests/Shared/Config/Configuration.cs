using Microsoft.Extensions.Configuration;

namespace Tech.Challenge.Persistence.Functional.Tests.Shared.Config;

public static class Configuration
{
    private static readonly string _environment = Environment.GetEnvironmentVariable("env_application") ?? "Development";

    public static readonly IConfiguration _config =
        new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{_environment}.json", optional: true, reloadOnChange: true)
        .Build();


}
