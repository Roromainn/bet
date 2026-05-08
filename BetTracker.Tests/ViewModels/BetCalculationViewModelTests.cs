using System;
using Moq;
using Xunit;
using BetTracker.Core.Services;
using BetTracker.UI.ViewModels;

namespace BetTracker.Tests.ViewModels;

/// <summary>
/// Unit tests for CalculationViewModel using mocked IBetCalculationService.
/// Tests calculation logic and command execution.
/// </summary>
public class BetCalculationViewModelTests
{
    private readonly Mock<IBetCalculationService> _mockCalculationService;
    private readonly CalculationViewModel _viewModel;

    public BetCalculationViewModelTests()
    {
        _mockCalculationService = new Mock<IBetCalculationService>();
        _viewModel = new CalculationViewModel(_mockCalculationService.Object);
    }

    [Fact]
    public void CalculateLayBetEV_WithValidInputs_UpdatesLayBetEV()
    {
        // Arrange
        _viewModel.BackStake = 100m;
        _viewModel.BackOdds = 3.0m;
        _viewModel.LayStake = 150m;
        _viewModel.LayOdds = 1.5m;

        decimal expectedEV = 100m * (3.0m - 1) - 150m * 1.5m; // -25
        _mockCalculationService
            .Setup(s => s.CalculateLayBetEV(100m, 3.0m, 150m, 1.5m))
            .Returns(expectedEV);

        // Act
        _viewModel.CalculateLayBetEVCommand.Execute(null);

        // Assert
        Assert.Equal(expectedEV, _viewModel.LayBetEV);
        Assert.Null(_viewModel.ErrorMessage);
        _mockCalculationService.Verify(s => s.CalculateLayBetEV(100m, 3.0m, 150m, 1.5m), Times.Once);
    }

    [Fact]
    public void CalculateLayBetEV_WithInvalidInputs_SetsErrorMessage()
    {
        // Arrange
        _viewModel.BackOdds = -1m; // Invalid: negative odds

        _mockCalculationService
            .Setup(s => s.CalculateLayBetEV(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
            .Throws(new ArgumentException("Odds must be positive"));

        // Act
        _viewModel.CalculateLayBetEVCommand.Execute(null);

        // Assert
        Assert.NotNull(_viewModel.ErrorMessage);
        Assert.Contains("Calculation error", _viewModel.ErrorMessage);
    }

    [Fact]
    public void CalculateMatchedBetProfit_WithValidInputs_UpdatesProfit()
    {
        // Arrange
        _viewModel.Stake1 = 100m;
        _viewModel.Odds1 = 2.0m;
        _viewModel.Stake2 = 100m;
        _viewModel.Odds2 = 2.0m;

        decimal expectedProfit = 0m;
        _mockCalculationService
            .Setup(s => s.CalculateMatchedBetProfit(100m, 2.0m, 100m, 2.0m))
            .Returns(expectedProfit);

        // Act
        _viewModel.CalculateMatchedBetProfitCommand.Execute(null);

        // Assert
        Assert.Equal(expectedProfit, _viewModel.MatchedBetProfit);
        Assert.Null(_viewModel.ErrorMessage);
    }

    [Fact]
    public void CalculateCoverageLoss_WithValidInputs_UpdatesLoss()
    {
        // Arrange
        _viewModel.CoverageStake1 = 100m;
        _viewModel.CoverageOdds1 = 3.0m;
        _viewModel.CoverageStake2 = 100m;
        _viewModel.CoverageOdds2 = 1.5m;

        decimal expectedLoss = 50m;
        _mockCalculationService
            .Setup(s => s.CalculateCoverageLoss(100m, 3.0m, 100m, 1.5m))
            .Returns(expectedLoss);

        // Act
        _viewModel.CalculateCoverageLossCommand.Execute(null);

        // Assert
        Assert.Equal(expectedLoss, _viewModel.CoverageLoss);
        Assert.Null(_viewModel.ErrorMessage);
    }

    [Fact]
    public void Clear_ResetsAllProperties()
    {
        // Arrange
        _viewModel.BackStake = 200m;
        _viewModel.LayBetEV = -50m;
        _viewModel.ErrorMessage = "Some error";

        // Act
        _viewModel.ClearCommand.Execute(null);

        // Assert
        Assert.Equal(100m, _viewModel.BackStake);
        Assert.Equal(3.0m, _viewModel.BackOdds);
        Assert.Equal(0m, _viewModel.LayBetEV);
        Assert.Null(_viewModel.ErrorMessage);
    }
}
