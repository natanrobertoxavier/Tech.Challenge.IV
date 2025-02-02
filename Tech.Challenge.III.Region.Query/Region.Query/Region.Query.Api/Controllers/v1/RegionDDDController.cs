using Microsoft.AspNetCore.Mvc;
using Region.Query.Api.Filters;
using Region.Query.Application.UseCase.DDD.Recover;
using Region.Query.Communication.Request.Enum;
using Region.Query.Communication.Response;
using System.ComponentModel.DataAnnotations;

namespace Region.Query.Api.Controllers.v1;

[ServiceFilter(typeof(AuthenticatedUserAttribute))]
public class RegionDDDController : TechChallengeController
{
    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<ResponseRegionDDDJson>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverAll(
        [FromServices] IRecoverRegionDDDUseCase useCase)
    {
        var result = await useCase.RecoverAllAsync();

        return Ok(result);
    }

    [HttpGet]
    [Route("DDD/by-region")]
    [ProducesResponseType(typeof(Result<IEnumerable<ResponseRegionDDDJson>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverByRegion(
        [FromQuery][Required] RegionRequestEnum region,
        [FromServices] IRecoverRegionDDDUseCase useCase)
    {
        var result = await useCase.RecoverListDDDByRegionAsync(region);

        return Ok(result);
    }

    [HttpGet]
    [Route("there-is-ddd/{dDD}")]
    [ProducesResponseType(typeof(Result<ResponseThereIsDDDNumberJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ThereIsDDDNumber(
        [FromRoute] int dDD,
        [FromServices] IRecoverRegionDDDUseCase useCase)
    {
        var result = await useCase.ThereIsDDDNumberAsync(dDD);

        return Ok(result);
    }

    [HttpGet]
    [Route("recover-by-id/{id}")]
    [ProducesResponseType(typeof(Result<ResponseRegionDDDJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverById(
        [FromRoute] Guid id,
        [FromServices] IRecoverRegionDDDUseCase useCase)
    {
        var result = await useCase.RecoverByIdAsync(id);

        return Ok(result);
    }

    [HttpGet]
    [Route("recover-by-ddd/{ddd}")]
    [ProducesResponseType(typeof(Result<ResponseRegionDDDJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverByDDD(
        [FromRoute] int ddd,
        [FromServices] IRecoverRegionDDDUseCase useCase)
    {
        var result = await useCase.RecoverByDDDAsync(ddd);

        return Ok(result);
    }
}
