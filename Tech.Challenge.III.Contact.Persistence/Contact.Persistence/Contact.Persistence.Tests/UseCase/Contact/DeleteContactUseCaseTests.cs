using Contact.Persistence.Application.Services.LoggedUser;
using Contact.Persistence.Application.UseCase.Contact.Delete;
using Contact.Persistence.Domain.Messages.DomaiEvents;
using Contact.Persistence.Domain.ResultServices;
using Contact.Persistence.Domain.Services;
using MediatR;
using Moq;
using Serilog;
using TokenService.Manager.Controller;

namespace Contact.Persistence.Tests.UseCase.Contact;

public class DeleteContactUseCaseTests
{
    private readonly Mock<IContactQueryServiceApi> _mockContactQueryServiceApi;
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILoggedUser> _mockLoggedUser;
    private readonly Mock<TokenController> _mockTokenController;
    private readonly Mock<ILogger> _mockLogger;
    private readonly DeleteContactUseCase _deleteContactUseCase;

    public DeleteContactUseCaseTests()
    {
        _mockContactQueryServiceApi = new Mock<IContactQueryServiceApi>();
        _mockMediator = new Mock<IMediator>();
        _mockLoggedUser = new Mock<ILoggedUser>();
        _mockTokenController = new Mock<TokenController>(1000, "YTJVPzlCM3Q1KVkoPGM1PlgqNit8MjR4O3Jba1ZR");
        _mockLogger = new Mock<ILogger>();

        _deleteContactUseCase = new DeleteContactUseCase(
            _mockContactQueryServiceApi.Object,
            _mockMediator.Object,
            _mockLoggedUser.Object,
            _mockTokenController.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task DeleteContactAsync_Success_ReturnsSuccessMessage()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var loggedUser = new Domain.Entities.User(Guid.NewGuid(), DateTime.UtcNow, "Jhon Cena", "email@teste.com", "any-password");
        var contactResult = new Domain.ResultServices.Result<ContactResult>
        {
            IsSuccess = true,
            Error = string.Empty,
            Data = new ContactResult()
            {
                ContactId = contactId,
                Region = "Sudeste",
                FirstName = "Any",
                LastName = "Contact",
                DDD = 14,
                PhoneNumber = "98877-9981",
                Email = "anyemail@email.com",
            }
        };

        _mockLoggedUser.Setup(lu => lu.RecoverUser()).ReturnsAsync(loggedUser);

        _mockContactQueryServiceApi.Setup(c => c.RecoverContactByIdAsync(contactId, It.IsAny<string>()))
            .ReturnsAsync(contactResult);

        // Act
        var result = await _deleteContactUseCase.DeleteContactAsync(contactId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Deleção em processamento.", result.Data.Message);
        _mockMediator.Verify(m => m.Publish(It.IsAny<DeleteContactDomainEvent>(), default), Times.Once);
    }

    [Fact]
    public async Task DeleteContactAsync_Failure_WhenContactNotFound_ReturnsErrorMessage()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var loggedUser = new Domain.Entities.User(Guid.NewGuid(), DateTime.UtcNow, "Jhon Cena", "email@teste.com", "any-password");
        var contactResult = new Domain.ResultServices.Result<ContactResult>
        {
            IsSuccess = false,
            Error = "Any error"
        };

        _mockLoggedUser.Setup(lu => lu.RecoverUser()).ReturnsAsync(loggedUser);

        _mockContactQueryServiceApi.Setup(c => c.RecoverContactByIdAsync(contactId, It.IsAny<string>()))
            .ReturnsAsync(contactResult);

        // Act
        var result = await _deleteContactUseCase.DeleteContactAsync(contactId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("An error occurred when calling the Region.Query Api. Error: Any error", result.Errors[0]);
    }

    [Fact]
    public async Task DeleteContactAsync_Exception_ReturnsErrorMessage()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        _mockLoggedUser.Setup(lu => lu.RecoverUser()).ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _deleteContactUseCase.DeleteContactAsync(contactId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("There is an error: Unexpected error", result.Errors[0]);
    }
}
