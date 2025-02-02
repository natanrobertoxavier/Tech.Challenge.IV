namespace Contact.Persistence.Domain.Messages.DomaiEvents;
public class DeleteContactDomainEvent(Guid contactId) : DomainEvent
{
    public Guid ContactId { get; set; } = contactId;
}
