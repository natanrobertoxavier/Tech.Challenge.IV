using AutoMapper;
using Moq;
using Region.Query.Application.UseCase.DDD.Recover;
using Region.Query.Communication;
using Region.Query.Communication.Request.Enum;
using Region.Query.Communication.Response;
using Region.Query.Communication.Response.Enum;
using Region.Query.Domain.Entities;
using Region.Query.Domain.Repositories;
using Serilog;

namespace Region.Query.Tests.UseCase;
public class RecoverRegionDDDUseCaseTests
{
    private readonly Mock<IRegionDDDReadOnlyRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger> _mockLogger;
    private readonly RecoverRegionDDDUseCase _useCase;

    public RecoverRegionDDDUseCaseTests()
    {
        _mockRepository = new Mock<IRegionDDDReadOnlyRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger>();
        _useCase = new RecoverRegionDDDUseCase(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnMappedResult_WhenRecoverAllIsCalled()
    {
        // Arrange
        var dddList = new List<RegionDDD>
        {
            new() { Id = Guid.NewGuid(), DDD = 11, Region = RegionResponseEnum.Sudeste.GetDescription() },
            new() { Id = Guid.NewGuid(), DDD = 14, Region = RegionResponseEnum.Sudeste.GetDescription() },
        };

        var mappedResult = new List<ResponseRegionDDDJson>
        {
            new(Guid.NewGuid(), 11, RegionResponseEnum.Sudeste.GetDescription()),
            new(Guid.NewGuid(), 14, RegionResponseEnum.Sudeste.GetDescription()),
        };

        _mockRepository
            .Setup(repo => repo.RecoverAllAsync())
            .ReturnsAsync(dddList);

        _mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ResponseRegionDDDJson>>(dddList))
            .Returns(mappedResult);

        // Act
        var result = await _useCase.RecoverAllAsync();

        // Assert
        Assert.Equal(mappedResult, result.Data.RegionsDDD);
        _mockRepository.Verify(repo => repo.RecoverAllAsync(), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<IEnumerable<ResponseRegionDDDJson>>(dddList), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldReturnMappedResult_WhenRecoverListDDDByRegionIsCalled()
    {
        // Arrange
        var request = RegionRequestEnum.Sul;

        var dddList = new List<RegionDDD>
        {
            new() { Id = Guid.NewGuid(), DDD = 41, Region = RegionResponseEnum.Sul.GetDescription() },
            new() { Id = Guid.NewGuid(), DDD = 42, Region = RegionResponseEnum.Sul.GetDescription() },
        };


        var mappedResult = new List<ResponseRegionDDDJson>
        {
            new(Guid.NewGuid(), 41, RegionResponseEnum.Sul.GetDescription()),
            new(Guid.NewGuid(), 42, RegionResponseEnum.Sul.GetDescription()),
        };

        _mockRepository
            .Setup(repo => repo.RecoverListDDDByRegionAsync(request.GetDescription()))
            .ReturnsAsync(dddList);

        _mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ResponseRegionDDDJson>>(dddList))
            .Returns(mappedResult);

        // Act
        var result = await _useCase.RecoverListDDDByRegionAsync(request);

        // Assert
        Assert.Equal(mappedResult, result.Data.RegionsDDD);
        _mockRepository.Verify(repo => repo.RecoverListDDDByRegionAsync(request.GetDescription()), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<IEnumerable<ResponseRegionDDDJson>>(dddList), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldReturnEmptyList_WhenNoDataIsFound()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.RecoverAllAsync())
            .ReturnsAsync(new List<RegionDDD>());

        _mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ResponseRegionDDDJson>>(It.IsAny<IEnumerable<RegionDDD>>()))
            .Returns(new List<ResponseRegionDDDJson>());

        // Act
        var result = await _useCase.RecoverAllAsync();

        // Assert
        Assert.NotNull(result);
        _mockRepository.Verify(repo => repo.RecoverAllAsync(), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<IEnumerable<ResponseRegionDDDJson>>(It.IsAny<IEnumerable<RegionDDD>>()), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldReturnEmptyList_WhenRecoverListDDDByRegionReturnsEmpty()
    {
        // Arrange
        var request = RegionRequestEnum.Nordeste;

        _mockRepository
            .Setup(repo => repo.RecoverListDDDByRegionAsync(request.GetDescription()))
            .ReturnsAsync(new List<RegionDDD>());

        _mockMapper
            .Setup(mapper => mapper.Map<IEnumerable<ResponseRegionDDDJson>>(It.IsAny<IEnumerable<RegionDDD>>()))
            .Returns(new List<ResponseRegionDDDJson>());

        // Act
        var result = await _useCase.RecoverListDDDByRegionAsync(request);

        // Assert
        Assert.Empty(result.Data.RegionsDDD);
        _mockRepository.Verify(repo => repo.RecoverListDDDByRegionAsync(request.GetDescription()), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<IEnumerable<ResponseRegionDDDJson>>(It.IsAny<IEnumerable<RegionDDD>>()), Times.Once);
    }
}
