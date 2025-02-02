namespace Contact.Persistence.Domain.Messages;
public class BaseMessage
{
    public string MessageType { get; }

    protected BaseMessage()
    {
        MessageType = GetType().Name;
    }
}
