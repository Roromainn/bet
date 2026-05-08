using Microsoft.Extensions.DependencyInjection;
using BetTracker.Core.Services;
using BetTracker.Data;
using BetTracker.Data.Repositories;

namespace BetTracker.Tests;

public class DIContainerTests
{
    [Fact]
    public void ServiceCollection_WithExtensions_RegistersAllServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddScoped<BetTrackerContext>();
        services.AddBetTrackerRepositories();
        services.AddBetTrackerCoreServices();
        var provider = services.BuildServiceProvider();

        // Assert
        // Verify repositories are registered
        var bookmakerRepo = provider.GetService<IBookmakerRepository>();
        Assert.NotNull(bookmakerRepo);

        var betRepo = provider.GetService<IBetRepository>();
        Assert.NotNull(betRepo);

        // Generic repository can be resolved for specific types
        // This is handled by the DI container for concrete uses

        // Verify services are registered
        var calculationService = provider.GetService<IBetCalculationService>();
        Assert.NotNull(calculationService);
    }

    [Fact]
    public void BetCalculationService_CanBeResolved_FromDIContainer()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddBetTrackerCoreServices();
        var provider = services.BuildServiceProvider();
        var service = provider.GetService<IBetCalculationService>();

        // Assert
        Assert.NotNull(service);
        Assert.IsType<BetCalculationService>(service);
    }

    [Fact]
    public void BookmakerRepository_CanBeResolved_FromDIContainer()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddScoped<BetTrackerContext>();
        services.AddBetTrackerRepositories();
        var provider = services.BuildServiceProvider();
        var repo = provider.GetService<IBookmakerRepository>();

        // Assert
        Assert.NotNull(repo);
        Assert.IsType<BookmakerRepository>(repo);
    }

    [Fact]
    public void BetRepository_CanBeResolved_FromDIContainer()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddScoped<BetTrackerContext>();
        services.AddBetTrackerRepositories();
        var provider = services.BuildServiceProvider();
        var repo = provider.GetService<IBetRepository>();

        // Assert
        Assert.NotNull(repo);
        Assert.IsType<BetRepository>(repo);
    }
}
