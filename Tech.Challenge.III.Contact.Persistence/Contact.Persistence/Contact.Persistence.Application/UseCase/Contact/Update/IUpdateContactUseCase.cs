using Contact.Persistence.Communication.Request;
using Contact.Persistence.Communication.Response;

namespace Contact.Persistence.Application.UseCase.Contact.Update;
public interface IUpdateContactUseCase
{
    Task<Result<MessageResult>> UpdateContact(Guid id, RequestContactJson request);
}
