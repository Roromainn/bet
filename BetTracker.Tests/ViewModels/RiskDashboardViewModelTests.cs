using System;
using Moq;
using Xunit;
using BetTracker.Core.Models;
using BetTracker.Core.Services;
using BetTracker.UI.ViewModels;

namespace BetTracker.Tests.ViewModels;

/// <summary>
/// Unit tests for RiskDashboardViewModel using mocked services.
/// Tests risk assessment calculations, liability, profit, and risk level determination.
/// </summary>
public class RiskDashboardViewModelTests
{
    private readonly Mock<IRiskAssessmentService> _mockRiskService;
    private readonly Mock<IProfitCalculatorService> _mockProfitService;
    private readonly RiskDashboardViewModel _viewModel;

    public RiskDashboardViewModelTests()
    {
        _mockRiskService = new Mock<IRiskAssessmentService>();
        _mockProfitService = new Mock<IProfitCalculatorService>();
        _viewModel = new RiskDashboardViewModel(_mockRiskService.Object, _mockProfitService.Object);
    }

    [Fact]
    public void Constructor_NullRiskService_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new RiskDashboardViewModel(null!, _mockProfitService.Object));
    }

    [Fact]
    public void Constructor_NullProfitService_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new RiskDashboardViewModel(_mockRiskService.Object, null!));
    }

    [Fact]
    public void Assess_CalculatesLiability_Correctly()
    {
        // Arrange
        _viewModel.LayStake = 50m;
        _viewModel.LayOdds = 2m;

        _mockProfitService
            .Setup(s => s.CalculateLiability(50m, 2m))
            .Returns(50m);

        _mockProfitService
            .Setup(s => s.CalculateNetProfit(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
            .Returns(0m);

        _mockRiskService
            .Setup(s => s.AssessRisk(It.IsAny<decimal>(), It.IsAny<decimal>()))
            .Returns(RiskLevel.Low);

        _mockRiskService
            .Setup(s => s.GetMaximumExposure(It.IsAny<decimal>()))
            .Returns(100m);

        // Act
        _viewModel.AssessCommand.Execute(null);

        // Assert
        Assert.Equal(50m, _viewModel.Liability);
    }

    [Fact]
    public void Assess_CalculatesNetProfit_Correctly()
    {
        // Arrange
        _viewModel.BackStake = 100m;
        _viewModel.BackOdds = 2.0m;
        _viewModel.LayStake = 100m;
        _viewModel.LayOdds = 1.5m;

        _mockProfitService
            .Setup(s => s.CalculateLiability(100m, 1.5m))
            .Returns(50m);

        _mockProfitService
            .Setup(s => s.CalculateNetProfit(100m, 2.0m, 100m, 1.5m))
            .Returns(50m);

        _mockRiskService
            .Setup(s => s.AssessRisk(100m, 50m))
            .Returns(RiskLevel.Low);

        _mockRiskService
            .Setup(s => s.GetMaximumExposure(50m))
            .Returns(100m);

        // Act
        _viewModel.AssessCommand.Execute(null);

        // Assert
        Assert.Equal(50m, _viewModel.NetProfit);
    }

    [Fact]
    public void Assess_SetsRiskLevelDisplay_Correctly()
    {
        // Arrange
        _mockProfitService
            .Setup(s => s.CalculateLiability(It.IsAny<decimal>(), It.IsAny<decimal>()))
            .Returns(100m);

        _mockProfitService
            .Setup(s => s.CalculateNetProfit(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
            .Returns(50m);

        _mockRiskService
            .Setup(s => s.AssessRisk(It.IsAny<decimal>(), It.IsAny<decimal>()))
            .Returns(RiskLevel.High);

        _mockRiskService
            .Setup(s => s.GetMaximumExposure(It.IsAny<decimal>()))
            .Returns(200m);

        // Act
        _viewModel.AssessCommand.Execute(null);

        // Assert
        Assert.Equal("High", _viewModel.RiskLevelDisplay);
    }
}
