using BetTracker.Core.Services;

namespace BetTracker.Tests.Services;

public class ProfitCalculatorServiceTests
{
    [Fact]
    public void CalculateNetProfit_ValidInputs_ReturnsCorrectValue()
    {
        // Arrange
        var service = new ProfitCalculatorService();
        decimal backStake = 100m;
        decimal backOdds = 2.5m;
        decimal layStake = 150m;
        decimal layOdds = 1.8m;

        // Act
        var profit = service.CalculateNetProfit(backStake, backOdds, layStake, layOdds);

        // Assert
        // backProfit = 100 * (2.5 - 1) = 150
        // layLiability = 150 * (1.8 - 1) = 120
        // netProfit = 150 - 120 = 30
        Assert.Equal(30m, profit);
    }

    [Fact]
    public void CalculateNetProfit_ZeroBackStake_ReturnsNegativeLiability()
    {
        // Arrange
        var service = new ProfitCalculatorService();
        decimal backStake = 0m;
        decimal backOdds = 2.5m;
        decimal layStake = 100m;
        decimal layOdds = 1.5m;

        // Act
        var profit = service.CalculateNetProfit(backStake, backOdds, layStake, layOdds);

        // Assert
        // backProfit = 0 * (2.5 - 1) = 0
        // layLiability = 100 * (1.5 - 1) = 50
        // netProfit = 0 - 50 = -50
        Assert.Equal(-50m, profit);
    }

    [Fact]
    public void CalculateLiability_ValidInputs_ReturnsCorrectValue()
    {
        // Arrange
        var service = new ProfitCalculatorService();
        decimal layStake = 200m;
        decimal layOdds = 2.0m;

        // Act
        var liability = service.CalculateLiability(layStake, layOdds);

        // Assert
        // liability = 200 * (2.0 - 1) = 200
        Assert.Equal(200m, liability);
    }

    [Fact]
    public void CalculateLiability_Odds1_ReturnsZero()
    {
        // Arrange
        var service = new ProfitCalculatorService();
        decimal layStake = 100m;
        decimal layOdds = 1m;

        // Act
        var liability = service.CalculateLiability(layStake, layOdds);

        // Assert
        // liability = 100 * (1 - 1) = 0
        Assert.Equal(0m, liability);
    }
}
