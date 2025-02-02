using Contact.Persistence.Communication.Response;

namespace Contact.Persistence.Application.UseCase.Contact.Delete;
public interface IDeleteContactUseCase
{
    Task<Result<MessageResult>> DeleteContactAsync(Guid id);
}
