using Contact.Persistence.Communication.Request;
using Contact.Persistence.Communication.Response;

namespace Contact.Persistence.Application.UseCase.Contact.Register;
public interface IRegisterContactUseCase
{
    Task<Result<MessageResult>> RegisterContactAsync(RequestContactJson request);
}
