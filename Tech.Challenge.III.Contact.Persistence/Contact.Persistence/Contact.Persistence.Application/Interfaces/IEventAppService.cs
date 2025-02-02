using Contact.Persistence.Domain.Messages.DomaiEvents;

namespace Contact.Persistence.Application.Interfaces;
public interface IEventAppService
{
    Task SendCreateContactEvent(ContactCreateDomainEvent message);
    Task SendDeleteContactEvent(DeleteContactDomainEvent message);
}
