using Contact.Persistence.Infrastructure.Queue;
using RabbitMq.Package.Events;

namespace Contact.Persistence.Application.Events.CreateContact;
public class OnContactCreateEvent : IRabbitMqEvent<OnContactCreateMessage>
{
    public string Exchange => RabbitMqConstants.ContactPersistenceExchange;

    public string RoutingKey => RabbitMqConstants.RegisterContactRoutingKey;

    public OnContactCreateMessage Payload { get; set; }
}
