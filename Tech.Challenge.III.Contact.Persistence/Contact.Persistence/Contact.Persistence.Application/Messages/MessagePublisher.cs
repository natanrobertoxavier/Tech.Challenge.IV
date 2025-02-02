using Contact.Persistence.Domain.Messages;
using MediatR;

namespace Contact.Persistence.Application.Messages;
public class MessagePublisher(IMediator mediatr) : IMessagePublisher
{
    private readonly IMediator _mediatr = mediatr;

    public async Task PublishDomainEvent<T>(T domainEvent) where T : DomainEvent
        => await _mediatr.Publish(domainEvent);
}
