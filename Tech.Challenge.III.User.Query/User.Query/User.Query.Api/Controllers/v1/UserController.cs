using Microsoft.AspNetCore.Mvc;
using User.Query.Application.UseCase.Recover;
using User.Query.Communication.Request;
using User.Query.Communication.Response;

namespace User.Query.Api.Controllers.v1;

public class UserController : TechChallengeController
{
    [HttpGet("{email}")]
    [ProducesResponseType(typeof(Result<ResponseUserJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverUserAsync(
        [FromServices] IRecoverUserUseCase useCase,
        [FromRoute] string email)
    {
        var result = await useCase.RecoverByEmailAsync(email);

        return Ok(result);
    }

    [HttpGet("there-is-user/{email}")]
    [ProducesResponseType(typeof(Result<ResponseExistsUserJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ThereIsUserAsync(
        [FromServices] IRecoverUserUseCase useCase,
        [FromRoute] string email)
    {
        var result = await useCase.ThereIsUserWithEmailAsync(email);

        return Ok(result);
    }

    [HttpPost("recover-email-password")]
    [ProducesResponseType(typeof(Result<ResponseUserJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverByEmailAndPasswordAsync(
        [FromServices] IRecoverUserUseCase useCase,
        [FromBody] RequestEmailPasswordUserJson request)
    {
        var result = await useCase.RecoverEmailPassword(request);

        if (result.IsSuccess)
            return Ok(result);

        return Unauthorized(result);
    }
}
