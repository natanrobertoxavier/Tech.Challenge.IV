using RabbitMq.Package.Events;

namespace Contact.Persistence.Infrastructure.Queue;
public interface IRabbitMqEventsDispatcher
{
    Task SendEvent<T>(IRabbitMqEvent<T> eventToSend);
}
