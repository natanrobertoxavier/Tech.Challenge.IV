using MediatR;

namespace Contact.Persistence.Domain.Messages;
public abstract class DomainEvent : BaseMessage, INotification
{
    protected DomainEvent() { }
}
