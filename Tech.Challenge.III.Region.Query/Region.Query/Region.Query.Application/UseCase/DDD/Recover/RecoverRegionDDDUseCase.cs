using AutoMapper;
using Region.Query.Communication;
using Region.Query.Communication.Request.Enum;
using Region.Query.Communication.Response;
using Region.Query.Domain.Entities;
using Region.Query.Domain.Repositories;
using Serilog;

namespace Region.Query.Application.UseCase.DDD.Recover;
public class RecoverRegionDDDUseCase(
    IRegionDDDReadOnlyRepository regionDDDReadOnlyRepository,
    IMapper mapper,
    ILogger logger) : IRecoverRegionDDDUseCase
{
    private readonly IRegionDDDReadOnlyRepository _regionDDDReadOnlyRepository = regionDDDReadOnlyRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;

    public async Task<Result<ResponseListRegionDDDJson>> RecoverAllAsync()
    {
        var output = new Result<ResponseListRegionDDDJson>();

        try
        {
            _logger.Information($"Start {nameof(RecoverAllAsync)}.");

            var result = await _regionDDDReadOnlyRepository.RecoverAllAsync();

            _logger.Information($"End {nameof(RecoverAllAsync)}.");

            return output.Success(new ResponseListRegionDDDJson(_mapper.Map<IEnumerable<ResponseRegionDDDJson>>(result)));
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format($"{nameof(RecoverAllAsync)} There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(errorMessage);
        }
    }

    public async Task<Result<ResponseListRegionDDDJson>> RecoverListDDDByRegionAsync(RegionRequestEnum request)
    {
        var output = new Result<ResponseListRegionDDDJson>();

        try
        {
            _logger.Information($"Start {nameof(RecoverListDDDByRegionAsync)}.");

            var result = await _regionDDDReadOnlyRepository.RecoverListDDDByRegionAsync(request.GetDescription());

            _logger.Information($"End {nameof(RecoverListDDDByRegionAsync)}.");

            return output.Success(new ResponseListRegionDDDJson(_mapper.Map<IEnumerable<ResponseRegionDDDJson>>(result)));
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format($"{nameof(RecoverListDDDByRegionAsync)} There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(errorMessage);
        }
    }

    public async Task<Result<ResponseThereIsDDDNumberJson>> ThereIsDDDNumberAsync(int dDD)
    {
        var output = new Result<ResponseThereIsDDDNumberJson>();

        try
        {
            _logger.Information($"Start {nameof(ThereIsDDDNumberAsync)}.");

            var result = await _regionDDDReadOnlyRepository.ThereIsDDDNumber(dDD);

            _logger.Information($"End {nameof(ThereIsDDDNumberAsync)}.");

            return output.Success(new ResponseThereIsDDDNumberJson(result));
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format($"{nameof(ThereIsDDDNumberAsync)} There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(errorMessage);
        }
    }

    public async Task<Result<ResponseRegionDDDJson>> RecoverByIdAsync(Guid id)
    {
        var output = new Result<ResponseRegionDDDJson>();

        try
        {
            _logger.Information($"Start {nameof(RecoverByIdAsync)}.");

            var result = await _regionDDDReadOnlyRepository.RecoverByIdAsync(id) ??
                new RegionDDD();

            _logger.Information($"End {nameof(RecoverByIdAsync)}.");

            return output.Success(new ResponseRegionDDDJson(result.Id, result.DDD, result.Region));
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format($"{nameof(RecoverByIdAsync)} There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(errorMessage);
        }
    }

    public async Task<Result<ResponseRegionDDDJson>> RecoverByDDDAsync(int ddd)
    {
        var output = new Result<ResponseRegionDDDJson>();

        try
        {
            _logger.Information($"Start {nameof(RecoverByDDDAsync)}.");

            var result = await _regionDDDReadOnlyRepository.RecoverByDDDAsync(ddd) ??
                new RegionDDD();

            _logger.Information($"End {nameof(RecoverByDDDAsync)}.");

            return output.Success(new ResponseRegionDDDJson(result.Id, result.DDD, result.Region));
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format($"{nameof(RecoverByDDDAsync)} There are an error: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            return output.Failure(errorMessage);
        }
    }
}
