using Contact.Persistence.Application.Interfaces;
using Contact.Persistence.Domain.Messages.DomaiEvents;
using MediatR;

namespace Contact.Persistence.Application.Messages.Handlers;
public class DeleteContactEventHandler(
    IEventAppService eventAppService) : INotificationHandler<DeleteContactDomainEvent>
{
    private readonly IEventAppService _eventAppService = eventAppService;

    public async Task Handle(DeleteContactDomainEvent message, CancellationToken cancellationToken)
    {
        await _eventAppService.SendDeleteContactEvent(message);
    }
}