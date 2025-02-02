using Contact.Persistence.Infrastructure.Queue;
using RabbitMq.Package.Events;

namespace Contact.Persistence.Application.Events.DeleteContact;
public class OnDeleteContactEvent : IRabbitMqEvent<OnDeleteContactMessage>
{
    public string Exchange => RabbitMqConstants.ContactPersistenceExchange;

    public string RoutingKey => RabbitMqConstants.DeleteContactRoutingKey;

    public OnDeleteContactMessage Payload { get; set; }
}
