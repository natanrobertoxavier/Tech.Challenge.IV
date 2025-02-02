using Region.Query.Communication.Request.Enum;
using Region.Query.Communication.Response;

namespace Region.Query.Application.UseCase.DDD.Recover;
public interface IRecoverRegionDDDUseCase
{
    Task<Result<ResponseListRegionDDDJson>> RecoverAllAsync();
    Task<Result<ResponseListRegionDDDJson>> RecoverListDDDByRegionAsync(RegionRequestEnum request);
    Task<Result<ResponseThereIsDDDNumberJson>> ThereIsDDDNumberAsync(int dDD);
    Task<Result<ResponseRegionDDDJson>> RecoverByIdAsync(Guid id);
    Task<Result<ResponseRegionDDDJson>> RecoverByDDDAsync(int ddd);
}
