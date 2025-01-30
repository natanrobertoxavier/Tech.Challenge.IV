using Microsoft.AspNetCore.Mvc;
using Moq;
using User.Query.Api.Controllers.v1;
using User.Query.Application.UseCase.Recover;
using User.Query.Communication.Response;

namespace User.Query.Tests.Controller;
public class UserControllerTests
{
    private readonly Mock<IRecoverUserUseCase> _recoverUserUseCaseMock;
    private readonly UserController _controller;
    private readonly Result<ResponseUserJson> output;

    public UserControllerTests()
    {
        _recoverUserUseCaseMock = new Mock<IRecoverUserUseCase>();
        _controller = new UserController();
        output = new Result<ResponseUserJson>();
    }

    [Fact]
    public async Task RecoverUser_ShouldReturnOk_WhenUseCaseSucceeds()
    {
        // Arrange
        var email = "test@example.com";

        var expectedResult = output.Success(new ResponseUserJson(Guid.NewGuid(), DateTime.UtcNow, "Test User", email, "anypasswordencrypted"));

        _recoverUserUseCaseMock
            .Setup(useCase => useCase.RecoverByEmailAsync(email))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.RecoverUserAsync(_recoverUserUseCaseMock.Object, email);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResult = Assert.IsType<Result<ResponseUserJson>>(okResult.Value);

        Assert.True(actualResult.IsSuccess);
        Assert.Equal(expectedResult.Data.Name, actualResult.Data.Name);
        Assert.Equal(expectedResult.Data.Email, actualResult.Data.Email);

        _recoverUserUseCaseMock.Verify(useCase => useCase.RecoverByEmailAsync(email), Times.Once);
    }

    [Fact]
    public async Task RecoverUser_ShouldReturnOkWithFailureResult_WhenUserNotFound()
    {
        // Arrange
        var email = "invalid@example.com";

        var expectedResult = output.Failure("User not found.");

        _recoverUserUseCaseMock
            .Setup(useCase => useCase.RecoverByEmailAsync(email))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.RecoverUserAsync(_recoverUserUseCaseMock.Object, email);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResult = Assert.IsType<Result<ResponseUserJson>>(okResult.Value);

        Assert.False(actualResult.IsSuccess);
        Assert.Equal(expectedResult.Error, actualResult.Error);

        _recoverUserUseCaseMock.Verify(useCase => useCase.RecoverByEmailAsync(email), Times.Once);
    }

    [Fact]
    public async Task RecoverUser_ShouldReturnOkWithFailureResult_WhenAnErrorOccurs()
    {
        // Arrange
        var email = "email@example.com";

        var expectedResult = output.Failure("Any Error.");

        _recoverUserUseCaseMock
            .Setup(useCase => useCase.RecoverByEmailAsync(email))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.RecoverUserAsync(_recoverUserUseCaseMock.Object, email);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResult = Assert.IsType<Result<ResponseUserJson>>(okResult.Value);

        Assert.False(actualResult.IsSuccess);
        Assert.Equal(expectedResult.Error, actualResult.Error);

        _recoverUserUseCaseMock.Verify(useCase => useCase.RecoverByEmailAsync(email), Times.Once);
    }
}
