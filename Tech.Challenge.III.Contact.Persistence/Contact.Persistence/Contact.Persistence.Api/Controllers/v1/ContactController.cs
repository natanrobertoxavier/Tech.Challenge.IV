using Contact.Persistence.Api.Filters;
using Contact.Persistence.Application.UseCase.Contact.Delete;
using Contact.Persistence.Application.UseCase.Contact.Register;
using Contact.Persistence.Application.UseCase.Contact.Update;
using Contact.Persistence.Communication.Request;
using Contact.Persistence.Communication.Response;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Contact.Persistence.Api.Controllers.v1;

[ServiceFilter(typeof(AuthenticatedUserAttribute))]
public class ContactController : TechChallengeController
{
    [HttpPost]
    [ProducesResponseType(typeof(Communication.Response.Result<MessageResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterContact(
        [FromServices] IRegisterContactUseCase useCase,
        [FromBody] RequestContactJson request)
    {
        var result = await useCase.RegisterContactAsync(request);

        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(typeof(Communication.Response.Result<MessageResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(
        [FromQuery][Required] Guid id,
        [FromBody][Required] RequestContactJson request,
        [FromServices] IUpdateContactUseCase useCase)
    {
        var result = await useCase.UpdateContact(id, request);

        return Ok(result);
    }

    [HttpDelete]
    [ProducesResponseType(typeof(Communication.Response.Result<MessageResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(
        [FromQuery][Required] Guid id,
        [FromServices] IDeleteContactUseCase useCase)
    {
        var result = await useCase.DeleteContactAsync(id);

        return Ok(result);
    }
}
