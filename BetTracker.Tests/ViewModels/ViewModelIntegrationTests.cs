using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using BetTracker.Core.Models;
using BetTracker.Core.Services;
using BetTracker.Data;
using BetTracker.Data.Repositories;
using BetTracker.UI.ViewModels;

namespace BetTracker.Tests.ViewModels;

/// <summary>
/// Integration tests for ViewModels and DI container resolution.
/// Verifies that ViewModels can be resolved and used with real services.
/// </summary>
public class ViewModelIntegrationTests
{
    private IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        // Register DbContext
        services.AddScoped<BetTrackerContext>();

        // Register repositories and services
        services.AddBetTrackerRepositories();
        services.AddBetTrackerCoreServices();

        // Register ViewModels
        services.AddTransient<BetsViewModel>();
        services.AddTransient<BookmakerViewModel>();
        services.AddTransient<CalculationViewModel>();

        return services.BuildServiceProvider();
    }

    [Fact]
    public void BetsViewModel_CanBeResolvedFromDI()
    {
        // Arrange
        var provider = BuildServiceProvider();

        // Act
        var viewModel = provider.GetRequiredService<BetsViewModel>();

        // Assert
        Assert.NotNull(viewModel);
    }

    [Fact]
    public void BookmakerViewModel_CanBeResolvedFromDI()
    {
        // Arrange
        var provider = BuildServiceProvider();

        // Act
        var viewModel = provider.GetRequiredService<BookmakerViewModel>();

        // Assert
        Assert.NotNull(viewModel);
    }

    [Fact]
    public void CalculationViewModel_CanBeResolvedFromDI()
    {
        // Arrange
        var provider = BuildServiceProvider();

        // Act
        var viewModel = provider.GetRequiredService<CalculationViewModel>();

        // Assert
        Assert.NotNull(viewModel);
    }

    [Fact]
    public void CalculationViewModel_CalculateLayBetEV_WithRealService_ReturnsCorrectValue()
    {
        // Arrange
        var provider = BuildServiceProvider();
        var viewModel = provider.GetRequiredService<CalculationViewModel>();

        viewModel.BackStake = 100m;
        viewModel.BackOdds = 3.0m;
        viewModel.LayStake = 150m;
        viewModel.LayOdds = 1.5m;

        // Act
        viewModel.CalculateLayBetEVCommand.Execute(null);

        // Assert
        Assert.Equal(-25m, viewModel.LayBetEV); // 100 * (3.0 - 1) - 150 * 1.5 = 200 - 225 = -25
        Assert.Null(viewModel.ErrorMessage);
    }

    [Fact]
    public void CalculationViewModel_CalculateMatchedBetProfit_ReturnsCorrectValue()
    {
        // Arrange
        var provider = BuildServiceProvider();
        var viewModel = provider.GetRequiredService<CalculationViewModel>();

        viewModel.Stake1 = 100m;
        viewModel.Odds1 = 2.0m;
        viewModel.Stake2 = 100m;
        viewModel.Odds2 = 2.0m;

        // Act
        viewModel.CalculateMatchedBetProfitCommand.Execute(null);

        // Assert
        Assert.Equal(0m, viewModel.MatchedBetProfit);
        Assert.Null(viewModel.ErrorMessage);
    }

    [Fact]
    public void Clear_ResetsViewModelState()
    {
        // Arrange
        var provider = BuildServiceProvider();
        var viewModel = provider.GetRequiredService<CalculationViewModel>();

        viewModel.BackStake = 500m;
        viewModel.LayBetEV = 100m;
        viewModel.ErrorMessage = "Test error";

        // Act
        viewModel.ClearCommand.Execute(null);

        // Assert
        Assert.Equal(100m, viewModel.BackStake); // Default value
        Assert.Equal(0m, viewModel.LayBetEV);
        Assert.Null(viewModel.ErrorMessage);
    }
}
