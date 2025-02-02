using Contact.Persistence.Api;
using Contact.Persistence.Api.Filters;
using Contact.Persistence.Application;
using Contact.Persistence.Application.Services.Automapper;
using Contact.Persistence.Communication;
using Contact.Persistence.Domain.Extensions;
using Contact.Persistence.Infrastructure;
using Contact.Persistence.Infrastructure.Migrations;
using Contact.Persistence.Infrastructure.RepositoryAccess;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new DescriptionEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer",
        new Microsoft.OpenApi.Models
        .OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "JWT Authorization header utilizando o Bearer sheme. Exemple: \"Authorization: Bearer {token}\"",
        });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    c.SchemaFilter<EnumSchemaFilter>();
});

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication(builder.Configuration);

builder.Services.AddTokenManager(builder.Configuration);

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilters)));

builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new TechChallengeProfile());
}).CreateMapper());

builder.Services.AddScoped<AuthenticatedUserAttribute>();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

UpdateDatabase();

app.UseAuthorization();

app.MapControllers();

app.Run();

void UpdateDatabase()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    using var context = serviceScope.ServiceProvider.GetService<TechChallengeContext>();

    bool? databaseInMemory = context?.Database?.ProviderName?.Equals("Microsoft.EntityFrameworkCore.InMemory");

    if (!databaseInMemory.HasValue || !databaseInMemory.Value)
    {
        var connection = builder.Configuration.GetConnection();
        var nomeDatabase = builder.Configuration.GetDatabaseName();

        Database.CreateDatabase(connection, nomeDatabase);

        app.MigrateDatabase();
    }
}

public partial class Program { }