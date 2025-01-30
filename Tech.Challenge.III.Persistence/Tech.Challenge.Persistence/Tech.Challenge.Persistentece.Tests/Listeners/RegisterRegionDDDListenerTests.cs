using Microsoft.Extensions.DependencyInjection;
using Moq;
using RabbitMQ.Client;
using Serilog;
using Tech.Challenge.Persistence.Api.Models;
using Tech.Challenge.Persistence.Domain.Entities;
using Tech.Challenge.Persistence.Domain.Repositories;
using Tech.Challenge.Persistence.Domain.Repositories.Region;

namespace Tech.Challenge.Persistentece.Tests.Listeners;

public class RegisterRegionDDDListenerTests
{
    private readonly Mock<IConnectionFactory> _connectionFactoryMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly Mock<IServiceScopeFactory> _scopeFactoryMock;
    private readonly Mock<IServiceScope> _scopeMock;
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IWorkUnit> _workUnitMock;
    private readonly Mock<IRegionDDDReadOnlyRepository> _regionReadOnlyRepositoryMock;
    private readonly Mock<IRegionWriteOnlyRepository> _regionWriteOnlyRepositoryMock;

    public RegisterRegionDDDListenerTests()
    {
        var connectionMock = new Mock<IConnection>();
        var channelMock = new Mock<IModel>();

        _connectionFactoryMock = new Mock<IConnectionFactory>();
        _loggerMock = new Mock<ILogger>();
        _scopeFactoryMock = new Mock<IServiceScopeFactory>();
        _scopeMock = new Mock<IServiceScope>();
        _serviceProviderMock = new Mock<IServiceProvider>();
        _workUnitMock = new Mock<IWorkUnit>();
        _regionReadOnlyRepositoryMock = new Mock<IRegionDDDReadOnlyRepository>();
        _regionWriteOnlyRepositoryMock = new Mock<IRegionWriteOnlyRepository>();

        _scopeFactoryMock.Setup(f => f.CreateScope()).Returns(_scopeMock.Object);
        _scopeMock.Setup(s => s.ServiceProvider).Returns(_serviceProviderMock.Object);

        _serviceProviderMock.Setup(p => p.GetService(typeof(IWorkUnit))).Returns(_workUnitMock.Object);
        _serviceProviderMock.Setup(p => p.GetService(typeof(IRegionDDDReadOnlyRepository))).Returns(_regionReadOnlyRepositoryMock.Object);
        _serviceProviderMock.Setup(p => p.GetService(typeof(IRegionWriteOnlyRepository))).Returns(_regionWriteOnlyRepositoryMock.Object);

        _connectionFactoryMock
            .Setup(factory => factory.CreateConnection())
            .Returns(Mock.Of<IConnection>());

        connectionMock
            .Setup(connection => connection.CreateModel())
            .Returns(channelMock.Object);

        _connectionFactoryMock
            .Setup(factory => factory.CreateConnection())
            .Returns(connectionMock.Object);
    }

    [Fact]
    public async Task ProcessMessageAsync_Should_RegisterDDD_When_DDDDoesNotExist()
    {
        // Arrange
        var message = new RegisterRegionDDDModel { DDD = 21, Region = "RegionName" };

        _regionReadOnlyRepositoryMock.Setup(r => r.ThereIsDDDNumber(message.DDD)).ReturnsAsync(false);

        var listener = new RegisterRegionDDDListenerForTesting(
            _connectionFactoryMock.Object,
            _loggerMock.Object,
            _scopeFactoryMock.Object);

        // Act
        await listener.ProcessMessageAsync(message);

        // Assert
        _regionWriteOnlyRepositoryMock.Verify(r => r.Add(It.IsAny<RegionDDD>()), Times.Once);
        _workUnitMock.Verify(w => w.Commit(), Times.Once);
        _loggerMock.Verify(l => l.Information(It.Is<string>(s => s.Contains("Region DDD register processing completed."))), Times.Once);
    }

    [Fact]
    public async Task ProcessMessageAsync_Should_LogAndSkip_When_DDDAlreadyExists()
    {
        // Arrange
        var message = new RegisterRegionDDDModel { DDD = 21, Region = "RegionName" };

        _regionReadOnlyRepositoryMock.Setup(r => r.ThereIsDDDNumber(message.DDD)).ReturnsAsync(true);

        var listener = new RegisterRegionDDDListenerForTesting(
            _connectionFactoryMock.Object,
            _loggerMock.Object,
            _scopeFactoryMock.Object);

        // Act
        await listener.ProcessMessageAsync(message);

        // Assert
        _regionWriteOnlyRepositoryMock.Verify(r => r.Add(It.IsAny<RegionDDD>()), Times.Never);
        _workUnitMock.Verify(w => w.Commit(), Times.Never);
        _loggerMock.Verify(l => l.Information(It.Is<string>(s => s.Contains("DDD already registered"))), Times.Once);
    }

    [Fact]
    public async Task ProcessMessageAsync_Should_Handle_GenericException()
    {
        // Arrange
        var message = new RegisterRegionDDDModel { DDD = 21, Region = "RegionName" };

        _regionReadOnlyRepositoryMock.Setup(r => r.ThereIsDDDNumber(message.DDD)).ThrowsAsync(new Exception("Test exception"));

        var listener = new RegisterRegionDDDListenerForTesting(
            _connectionFactoryMock.Object,
            _loggerMock.Object,
            _scopeFactoryMock.Object);

        // Act
        await listener.ProcessMessageAsync(message);

        // Assert
        _loggerMock.Verify(l => l.Error(It.Is<string>(s => s.Contains("An error occurred while processing the message."))), Times.Once);
        _loggerMock.Verify(l => l.Fatal(It.Is<string>(s => s.Contains("Critical error occurred"))), Times.Once);
    }
}
