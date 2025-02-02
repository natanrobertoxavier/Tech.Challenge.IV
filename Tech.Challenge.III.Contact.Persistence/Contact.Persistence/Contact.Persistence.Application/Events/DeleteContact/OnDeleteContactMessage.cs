namespace Contact.Persistence.Application.Events.DeleteContact;
public class OnDeleteContactMessage(Guid contactId)
{
    public Guid ContactId { get; set; } = contactId;
}
