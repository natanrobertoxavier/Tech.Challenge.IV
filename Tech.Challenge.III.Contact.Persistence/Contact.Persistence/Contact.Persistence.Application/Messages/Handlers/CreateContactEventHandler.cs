using Contact.Persistence.Application.Interfaces;
using Contact.Persistence.Domain.Messages.DomaiEvents;
using MediatR;

namespace Contact.Persistence.Application.Messages.Handlers;
public class CreateContactEventHandler(
    IEventAppService eventAppService) : INotificationHandler<ContactCreateDomainEvent>
{
    private readonly IEventAppService _eventAppService = eventAppService;

    public async Task Handle(ContactCreateDomainEvent message, CancellationToken cancellationToken)
    {
        await _eventAppService.SendCreateContactEvent(message);
    }
}
