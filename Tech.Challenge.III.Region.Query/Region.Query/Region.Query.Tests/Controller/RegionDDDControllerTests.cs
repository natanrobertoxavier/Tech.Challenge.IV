using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Region.Query.Api.Controllers.v1;
using Region.Query.Application.UseCase.DDD.Recover;
using Region.Query.Communication;
using Region.Query.Communication.Request.Enum;
using Region.Query.Communication.Response;
using Region.Query.Communication.Response.Enum;

namespace Region.Query.Tests.Controller;
public class RegionDDDControllerTests
{
    [Fact]
    public async Task RecoverAll_ReturnsOkResult_WhenDataExists()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverRegionDDDUseCase>();

        var mockResponse = new ResponseListRegionDDDJson(new List<ResponseRegionDDDJson>
        {
            new ResponseRegionDDDJson(Guid.NewGuid(), 11, RegionResponseEnum.Sudeste.GetDescription()),
            new ResponseRegionDDDJson(Guid.NewGuid(),21, RegionResponseEnum.Sudeste.GetDescription())
        });

        var response = new Communication.Response.Result<ResponseListRegionDDDJson>(
            mockResponse, true, string.Empty);

        mockUseCase.Setup(useCase => useCase.RecoverAllAsync())
                   .ReturnsAsync(response);

        var controller = new RegionDDDController();

        // Act
        var result = await controller.RecoverAll(mockUseCase.Object) as OkObjectResult;

        // Assert
        var actualResult = Assert.IsType<Communication.Response.Result<ResponseListRegionDDDJson>>(result.Value);

        Assert.NotNull(result);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task RecoverAll_ReturnsOkResult_WhenNoDataExists()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverRegionDDDUseCase>();

        mockUseCase.Setup(useCase => useCase.RecoverAllAsync())
                   .ReturnsAsync(new Communication.Response.Result<ResponseListRegionDDDJson>(
            new ResponseListRegionDDDJson(null), true, string.Empty));

        var controller = new RegionDDDController();

        // Act
        var result = await controller.RecoverAll(mockUseCase.Object) as OkObjectResult;

        // Assert
        var actualResult = Assert.IsType<Communication.Response.Result<ResponseListRegionDDDJson>>(result.Value);

        Assert.NotNull(result);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task RecoverAll_ReturnsUnauthorizedResult_WhenUserIsNotAuthorized()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverRegionDDDUseCase>();

        mockUseCase.Setup(useCase => useCase.RecoverAllAsync())
                   .ThrowsAsync(new UnauthorizedAccessException("Usuário sem permissão"));

        var controller = new RegionDDDController();

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            controller.RecoverAll(mockUseCase.Object));

        // Assert
        Assert.Equal("Usuário sem permissão", exception.Message);
    }

    [Fact]
    public async Task RecoverByRegion_ReturnsOkResult_WhenDataExists()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverRegionDDDUseCase>();

        var region = RegionRequestEnum.Sudeste;

        var mockResponse = new ResponseListRegionDDDJson(new List<ResponseRegionDDDJson>
        {
            new ResponseRegionDDDJson(Guid.NewGuid(),11, RegionResponseEnum.Sudeste.GetDescription()),
            new ResponseRegionDDDJson(Guid.NewGuid(), 21, RegionResponseEnum.Sudeste.GetDescription())
        });

        var response = new Communication.Response.Result<ResponseListRegionDDDJson>(
            mockResponse, true, string.Empty);

        mockUseCase.Setup(useCase => useCase.RecoverListDDDByRegionAsync(region))
                   .ReturnsAsync(response);

        var controller = new RegionDDDController();

        // Act
        var result = await controller.RecoverByRegion(region, mockUseCase.Object) as OkObjectResult;

        // Assert
        var actualResult = Assert.IsType<Communication.Response.Result<ResponseListRegionDDDJson>>(result.Value);

        Assert.NotNull(result);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task RecoverByRegion_ReturnsOkResult_WhenNoDataExists()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverRegionDDDUseCase>();

        var region = RegionRequestEnum.Sudeste;

        mockUseCase.Setup(useCase => useCase.RecoverListDDDByRegionAsync(region))
                   .ReturnsAsync(new Communication.Response.Result<ResponseListRegionDDDJson>(
            new ResponseListRegionDDDJson(null), true, string.Empty));

        var controller = new RegionDDDController();

        // Act
        var result = await controller.RecoverByRegion(region, mockUseCase.Object) as OkObjectResult;

        // Assert
        var actualResult = Assert.IsType<Communication.Response.Result<ResponseListRegionDDDJson>>(result.Value);

        Assert.NotNull(result);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task RecoverByRegion_ReturnsUnauthorizedResult_WhenUserIsNotAuthorized()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverRegionDDDUseCase>();

        var region = RegionRequestEnum.Sudeste;

        mockUseCase.Setup(useCase => useCase.RecoverListDDDByRegionAsync(region))
                   .ThrowsAsync(new UnauthorizedAccessException("Usuário sem permissão"));

        var controller = new RegionDDDController();

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            controller.RecoverByRegion(region, mockUseCase.Object));

        // Assert
        Assert.Equal("Usuário sem permissão", exception.Message);
    }

    [Fact]
    public async Task ThereIsDDDNumber_ReturnsOkResult_WhenDataExists()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverRegionDDDUseCase>();

        var region = 11;

        var response = new Communication.Response.Result<ResponseThereIsDDDNumberJson>(
            new ResponseThereIsDDDNumberJson(true), true, string.Empty);

        mockUseCase.Setup(useCase => useCase.ThereIsDDDNumberAsync(region))
                   .ReturnsAsync(response);

        var controller = new RegionDDDController();

        // Act
        var result = await controller.ThereIsDDDNumber(region, mockUseCase.Object) as OkObjectResult;

        // Assert
        var actualResult = Assert.IsType<Communication.Response.Result<ResponseThereIsDDDNumberJson>>(result.Value);

        Assert.NotNull(result);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task ThereIsDDDNumber_ReturnsOkResult_WhenNoDataExists()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverRegionDDDUseCase>();

        var region = 11;

        var response = new Communication.Response.Result<ResponseThereIsDDDNumberJson>(
            new ResponseThereIsDDDNumberJson(false), true, string.Empty);

        mockUseCase.Setup(useCase => useCase.ThereIsDDDNumberAsync(region))
                   .ReturnsAsync(response);

        var controller = new RegionDDDController();

        // Act
        var result = await controller.ThereIsDDDNumber(region, mockUseCase.Object) as OkObjectResult;

        // Assert
        var actualResult = Assert.IsType<Communication.Response.Result<ResponseThereIsDDDNumberJson>>(result.Value);

        Assert.NotNull(result);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task ThereIsDDDNumber_ReturnsUnauthorizedResult_WhenUserIsNotAuthorized()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverRegionDDDUseCase>();

        var region = 11;

        mockUseCase.Setup(useCase => useCase.ThereIsDDDNumberAsync(region))
                   .ThrowsAsync(new UnauthorizedAccessException("Usuário sem permissão"));

        var controller = new RegionDDDController();

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            controller.ThereIsDDDNumber(region, mockUseCase.Object));

        // Assert
        Assert.Equal("Usuário sem permissão", exception.Message);
    }

    [Fact]
    public async Task RecoverById_ReturnsOkResult_WhenDataExists()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverRegionDDDUseCase>();

        var region = Guid.NewGuid();

        var response = new Communication.Response.Result<ResponseRegionDDDJson>(
            new ResponseRegionDDDJson(Guid.NewGuid(), 11, RegionRequestEnum.Sudeste.GetDescription()), true, string.Empty);

        mockUseCase.Setup(useCase => useCase.RecoverByIdAsync(region))
                   .ReturnsAsync(response);

        var controller = new RegionDDDController();

        // Act
        var result = await controller.RecoverById(region, mockUseCase.Object) as OkObjectResult;

        // Assert
        var actualResult = Assert.IsType<Communication.Response.Result<ResponseRegionDDDJson>>(result.Value);

        Assert.NotNull(result);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task RecoverById_ReturnsOkResult_WhenNoDataExists()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverRegionDDDUseCase>();

        var region = Guid.NewGuid();

        var response = new Communication.Response.Result<ResponseRegionDDDJson>(
            null, true, string.Empty);

        mockUseCase.Setup(useCase => useCase.RecoverByIdAsync(region))
                   .ReturnsAsync(response);

        var controller = new RegionDDDController();

        // Act
        var result = await controller.RecoverById(region, mockUseCase.Object) as OkObjectResult;

        // Assert
        var actualResult = Assert.IsType<Communication.Response.Result<ResponseRegionDDDJson>>(result.Value);

        Assert.NotNull(result);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task RecoverById_ReturnsUnauthorizedResult_WhenUserIsNotAuthorized()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverRegionDDDUseCase>();

        var region = Guid.NewGuid();

        mockUseCase.Setup(useCase => useCase.RecoverByIdAsync(region))
                   .ThrowsAsync(new UnauthorizedAccessException("Usuário sem permissão"));

        var controller = new RegionDDDController();

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            controller.RecoverById(region, mockUseCase.Object));

        // Assert
        Assert.Equal("Usuário sem permissão", exception.Message);
    }

    [Fact]
    public async Task RecoverByDDD_ReturnsOkResult_WhenDataExists()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverRegionDDDUseCase>();

        var region = 11;

        var response = new Communication.Response.Result<ResponseRegionDDDJson>(
            new ResponseRegionDDDJson(Guid.NewGuid(), 11, RegionRequestEnum.Sudeste.GetDescription()), true, string.Empty);

        mockUseCase.Setup(useCase => useCase.RecoverByDDDAsync(region))
                   .ReturnsAsync(response);

        var controller = new RegionDDDController();

        // Act
        var result = await controller.RecoverByDDD(region, mockUseCase.Object) as OkObjectResult;

        // Assert
        var actualResult = Assert.IsType<Communication.Response.Result<ResponseRegionDDDJson>>(result.Value);

        Assert.NotNull(result);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task RecoverByDDD_ReturnsOkResult_WhenNoDataExists()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverRegionDDDUseCase>();

        var region = 11;

        var response = new Communication.Response.Result<ResponseRegionDDDJson>(
            null, true, string.Empty);

        mockUseCase.Setup(useCase => useCase.RecoverByDDDAsync(region))
                   .ReturnsAsync(response);

        var controller = new RegionDDDController();

        // Act
        var result = await controller.RecoverByDDD(region, mockUseCase.Object) as OkObjectResult;

        // Assert
        var actualResult = Assert.IsType<Communication.Response.Result<ResponseRegionDDDJson>>(result.Value);

        Assert.NotNull(result);
        Assert.True(actualResult.IsSuccess);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task RecoverByDDD_ReturnsUnauthorizedResult_WhenUserIsNotAuthorized()
    {
        // Arrange
        var mockUseCase = new Mock<IRecoverRegionDDDUseCase>();

        var region = 11;

        mockUseCase.Setup(useCase => useCase.RecoverByDDDAsync(region))
                   .ThrowsAsync(new UnauthorizedAccessException("Usuário sem permissão"));

        var controller = new RegionDDDController();

        // Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            controller.RecoverByDDD(region, mockUseCase.Object));

        // Assert
        Assert.Equal("Usuário sem permissão", exception.Message);
    }
}
