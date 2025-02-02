using Contact.Persistence.Application.Interfaces;
using Contact.Persistence.Application.Messages;
using Contact.Persistence.Application.Messages.Handlers;
using Contact.Persistence.Application.Services;
using Contact.Persistence.Application.Services.LoggedUser;
using Contact.Persistence.Application.UseCase.Contact.Delete;
using Contact.Persistence.Application.UseCase.Contact.Register;
using Contact.Persistence.Application.UseCase.Contact.Update;
using Contact.Persistence.Domain.Messages;
using Contact.Persistence.Domain.Messages.DomaiEvents;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TokenService.Manager.Controller;

namespace Contact.Persistence.Application;

public static class Initializer
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAdditionalKeyPassword(services, configuration);
        AddJWTToken(services, configuration);
        AddUseCases(services);
        AddLoggedUser(services);
        AddDomainEvents(services);
        AddEvents(services);
    }

    private static void AddAdditionalKeyPassword(IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection("Settings:Password:AdditionalKeyPassword");
        services.AddScoped(option => new PasswordEncryptor(section.Value));
    }

    private static void AddJWTToken(IServiceCollection services, IConfiguration configuration)
    {
        var sectionLifeTime = configuration.GetRequiredSection("Settings:Jwt:LifeTimeTokenMinutes");
        var sectionKey = configuration.GetRequiredSection("Settings:Jwt:KeyToken");
        services.AddScoped(option => new TokenController(int.Parse(sectionLifeTime.Value), sectionKey.Value));
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterContactUseCase, RegisterContactUseCase>();
        services.AddScoped<IUpdateContactUseCase, UpdateContactUseCase>();
        services.AddScoped<IDeleteContactUseCase, DeleteContactUseCase>();
    }
    private static void AddLoggedUser(IServiceCollection services)
    {
        services.AddScoped<ILoggedUser, LoggedUser>();
    }

    private static void AddDomainEvents(IServiceCollection services)
    {
        services.AddScoped<IMessagePublisher, MessagePublisher>();
        services.AddScoped<INotificationHandler<ContactCreateDomainEvent>, CreateContactEventHandler>();
        services.AddScoped<INotificationHandler<DeleteContactDomainEvent>, DeleteContactEventHandler>();
    }

    private static void AddEvents(IServiceCollection services)
    {
        services.AddScoped<IEventAppService, EventsAppService>();
    }
}
