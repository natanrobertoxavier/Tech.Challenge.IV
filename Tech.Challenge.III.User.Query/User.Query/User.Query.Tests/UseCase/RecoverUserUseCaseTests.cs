using Moq;
using Serilog;
using User.Query.Application.UseCase.Recover;
using User.Query.Communication.Request;
using User.Query.Domain.Repositories;

namespace User.Query.Tests.UseCase;
public class RecoverUserUseCaseTests
{
    private readonly Mock<IUserReadOnlyRepository> _mockUserReadOnlyRepository;
    private readonly Mock<ILogger> _mockLogger;
    private readonly RecoverUserUseCase _useCase;

    public RecoverUserUseCaseTests()
    {
        _mockUserReadOnlyRepository = new Mock<IUserReadOnlyRepository>();
        _mockLogger = new Mock<ILogger>();
        _useCase = new RecoverUserUseCase(_mockUserReadOnlyRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ThereIsUserWithEmail_ShouldReturnSuccess_WhenUserExists()
    {
        // Arrange
        var email = "test@example.com";

        _mockUserReadOnlyRepository
            .Setup(repo => repo.ThereIsUserWithEmailAsync(email))
            .ReturnsAsync(true);

        // Act
        var result = await _useCase.ThereIsUserWithEmailAsync(email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);

        _mockLogger.Verify(logger => logger.Information(It.Is<string>(s => s.Contains("Start"))), Times.Once);
        _mockLogger.Verify(logger => logger.Information(It.Is<string>(s => s.Contains("End"))), Times.Once);
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ThereIsUserWithEmail_ShouldReturnSuccess_WhenUserDoesNotExist()
    {
        // Arrange
        var email = "nonexistent@example.com";

        _mockUserReadOnlyRepository
            .Setup(repo => repo.ThereIsUserWithEmailAsync(email))
            .ReturnsAsync(false);

        // Act
        var result = await _useCase.ThereIsUserWithEmailAsync(email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);

        _mockLogger.Verify(logger => logger.Information(It.Is<string>(s => s.Contains("Start"))), Times.Once);
        _mockLogger.Verify(logger => logger.Information(It.Is<string>(s => s.Contains("End"))), Times.Once);
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ThereIsUserWithEmail_ShouldReturnFailure_WhenExceptionIsThrown()
    {
        // Arrange
        var email = "test@example.com";

        var exceptionMessage = "Database error";

        _mockUserReadOnlyRepository
            .Setup(repo => repo.ThereIsUserWithEmailAsync(email))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _useCase.ThereIsUserWithEmailAsync(email);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"There are an error: {exceptionMessage}", result.Error);

        _mockLogger.Verify(logger => logger.Information(It.Is<string>(s => s.Contains("Start"))), Times.Once);
        _mockLogger.Verify(logger => logger.Error(It.IsAny<Exception>(), It.Is<string>(s => s.Contains(exceptionMessage))), Times.Once);
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RecoverByEmail_ShouldReturnSuccess_WhenUserExists()
    {
        // Arrange
        var email = "test@example.com";

        var user = new Domain.Entities.User { Name = "Test User", Email = email, Password = "AnyPassword" };

        _mockUserReadOnlyRepository
            .Setup(repo => repo.RecoverByEmailAsync(email))
            .ReturnsAsync(user);

        // Act
        var result = await _useCase.RecoverByEmailAsync(email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(user.Name, result.Data.Name);
        Assert.Equal(user.Email, result.Data.Email);

        _mockLogger.Verify(logger => logger.Information(It.Is<string>(s => s.Contains("Start"))), Times.Once);
        _mockLogger.Verify(logger => logger.Information(It.Is<string>(s => s.Contains("End"))), Times.Once);
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RecoverByEmail_ShouldReturnFailure_WhenUserDoesNotExist()
    {
        // Arrange
        var email = "notfound@example.com";

        _mockUserReadOnlyRepository
            .Setup(repo => repo.RecoverByEmailAsync(email))
            .ReturnsAsync((Domain.Entities.User)null);

        // Act
        var result = await _useCase.RecoverByEmailAsync(email);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"No user found with email: {email}", result.Error);

        _mockLogger.Verify(logger => logger.Information(It.Is<string>(s => s.Contains("Start"))), Times.Once);
        _mockLogger.Verify(logger => logger.Warning(It.Is<string>(s => s.Contains($"No user found with email: {email}"))), Times.Once);
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RecoverByEmail_ShouldReturnFailure_WhenExceptionIsThrown()
    {
        // Arrange
        var email = "test@example.com";

        var exceptionMessage = "Database error";

        _mockUserReadOnlyRepository
            .Setup(repo => repo.RecoverByEmailAsync(email))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _useCase.RecoverByEmailAsync(email);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"There are an error: {exceptionMessage}", result.Error);

        _mockLogger.Verify(logger => logger.Information(It.Is<string>(s => s.Contains("Start"))), Times.Once);
        _mockLogger.Verify(logger => logger.Error(It.IsAny<Exception>(), It.Is<string>(s => s.Contains(exceptionMessage))), Times.Once);
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RecoverEmailPassword_ShouldReturnSuccess_WhenUserExists()
    {
        // Arrange
        var request = new RequestEmailPasswordUserJson("test@example.com", "password123");

        var user = new Domain.Entities.User { Name = "Test User", Email = request.Email };

        _mockUserReadOnlyRepository
            .Setup(repo => repo.RecoverEmailPasswordAsync(request.Email, request.Password))
            .ReturnsAsync(user);

        // Act
        var result = await _useCase.RecoverEmailPassword(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(user.Name, result.Data.Name);
        Assert.Equal(user.Email, result.Data.Email);

        _mockLogger.Verify(logger => logger.Information(It.Is<string>(s => s.Contains("Start"))), Times.Once);
        _mockLogger.Verify(logger => logger.Information(It.Is<string>(s => s.Contains("End"))), Times.Once);
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RecoverEmailPassword_ShouldReturnFailure_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new RequestEmailPasswordUserJson("test@example.com", "password123");

        _mockUserReadOnlyRepository
            .Setup(repo => repo.RecoverEmailPasswordAsync(request.Email, request.Password))
            .ReturnsAsync((Domain.Entities.User)null);

        // Act
        var result = await _useCase.RecoverEmailPassword(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Email ou senha inválidos.", result.Error);

        _mockLogger.Verify(logger => logger.Information(It.Is<string>(s => s.Contains("Start"))), Times.Once);
        _mockLogger.Verify(logger => logger.Warning(It.Is<string>(s => s.Contains("Email ou senha inválidos."))), Times.Once);
        _mockLogger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task RecoverEmailPassword_ShouldReturnFailure_WhenExceptionIsThrown()
    {
        // Arrange
        var request = new RequestEmailPasswordUserJson("test@example.com", "password123");

        var exceptionMessage = "Database error";

        _mockUserReadOnlyRepository
            .Setup(repo => repo.RecoverEmailPasswordAsync(request.Email, request.Password))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _useCase.RecoverEmailPassword(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal($"There are an error: {exceptionMessage}", result.Error);

        _mockLogger.Verify(logger => logger.Information(It.Is<string>(s => s.Contains("Start"))), Times.Once);
        _mockLogger.Verify(logger => logger.Error(It.IsAny<Exception>(), It.Is<string>(s => s.Contains(exceptionMessage))), Times.Once);
        _mockLogger.VerifyNoOtherCalls();
    }
}
