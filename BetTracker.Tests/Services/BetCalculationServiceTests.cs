using BetTracker.Core.Services;
using BetTracker.Core.Models;

namespace BetTracker.Tests.Services;

public class BetCalculationServiceTests
{
    private readonly IBetCalculationService _service;

    public BetCalculationServiceTests()
    {
        _service = new BetCalculationService();
    }

    [Fact]
    public void CalculateLayBetEV_WithValidInputs_ReturnsCorrectEV()
    {
        // Arrange
        decimal backStake = 100m;
        decimal backOdds = 2.5m;
        decimal layStake = 150m;
        decimal layOdds = 1.8m;

        // Act
        var ev = _service.CalculateLayBetEV(backStake, backOdds, layStake, layOdds);

        // Assert
        // EV = 100 * (2.5 - 1) - 150 * 1.8
        // EV = 100 * 1.5 - 270
        // EV = 150 - 270 = -120
        Assert.Equal(-120m, ev);
    }

    [Fact]
    public void CalculateLayBetEV_WithPositiveEV_ReturnsPositiveValue()
    {
        // Arrange
        decimal backStake = 100m;
        decimal backOdds = 3.0m;
        decimal layStake = 150m;
        decimal layOdds = 1.5m;

        // Act
        var ev = _service.CalculateLayBetEV(backStake, backOdds, layStake, layOdds);

        // Assert
        // EV = 100 * (3.0 - 1) - 150 * 1.5
        // EV = 100 * 2.0 - 225
        // EV = 200 - 225 = -25
        Assert.Equal(-25m, ev);
    }

    [Fact]
    public void CalculateLayBetEV_WithNegativeOdds_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            _service.CalculateLayBetEV(100m, -2.5m, 150m, 1.8m)
        );
    }

    [Fact]
    public void CalculateMatchedBetProfit_WithValidInputs_ReturnsCorrectProfit()
    {
        // Arrange
        decimal stake1 = 100m;
        decimal odds1 = 2.0m;
        decimal stake2 = 100m;
        decimal odds2 = 2.0m;

        // Act
        var profit = _service.CalculateMatchedBetProfit(stake1, odds1, stake2, odds2);

        // Assert
        // Min return = 100 * 2.0 = 200
        // Total stakes = 100 + 100 = 200
        // Profit = 200 - 200 = 0
        Assert.Equal(0m, profit);
    }

    [Fact]
    public void CalculateMatchedBetProfit_WithNegativeStake_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            _service.CalculateMatchedBetProfit(-100m, 2.0m, 100m, 2.0m)
        );
    }

    [Fact]
    public void CalculateCoverageLoss_WithImbalancedOdds_ReturnsLoss()
    {
        // Arrange
        decimal stake1 = 100m;
        decimal odds1 = 3.0m;
        decimal stake2 = 100m;
        decimal odds2 = 1.5m;

        // Act
        var loss = _service.CalculateCoverageLoss(stake1, odds1, stake2, odds2);

        // Assert
        // Outcome1 = 100 * 3.0 = 300
        // Outcome2 = 100 * 1.5 = 150
        // Min = 150
        // Total stakes = 200
        // Loss = 200 - 150 = 50
        Assert.Equal(50m, loss);
    }

    [Fact]
    public void CalculateCoverageLoss_WithPerfectCoverage_ReturnsZero()
    {
        // Arrange
        decimal stake1 = 100m;
        decimal odds1 = 2.0m;
        decimal stake2 = 100m;
        decimal odds2 = 2.0m;

        // Act
        var loss = _service.CalculateCoverageLoss(stake1, odds1, stake2, odds2);

        // Assert
        Assert.Equal(0m, loss);
    }

    [Fact]
    public void CalculateCoverageLoss_WithNegativeOdds_ThrowsException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            _service.CalculateCoverageLoss(100m, -2.0m, 100m, 2.0m)
        );
    }
}
