using BetTracker.Core.Models;
using BetTracker.Core.Services;

namespace BetTracker.Tests.Services;

public class RiskAssessmentServiceTests
{
    [Fact]
    public void AssessRisk_LowRatio_ReturnsLow()
    {
        // Arrange
        var service = new RiskAssessmentService();
        decimal totalBackStake = 400m;
        decimal totalLiability = 100m;
        // ratio = 100 / 400 = 0.25 (< 0.5)

        // Act
        var risk = service.AssessRisk(totalBackStake, totalLiability);

        // Assert
        Assert.Equal(RiskLevel.Low, risk);
    }

    [Fact]
    public void AssessRisk_MediumRatio_ReturnsMedium()
    {
        // Arrange
        var service = new RiskAssessmentService();
        decimal totalBackStake = 400m;
        decimal totalLiability = 300m;
        // ratio = 300 / 400 = 0.75 (0.5 - 1.0)

        // Act
        var risk = service.AssessRisk(totalBackStake, totalLiability);

        // Assert
        Assert.Equal(RiskLevel.Medium, risk);
    }

    [Fact]
    public void AssessRisk_HighRatio_ReturnsHigh()
    {
        // Arrange
        var service = new RiskAssessmentService();
        decimal totalBackStake = 200m;
        decimal totalLiability = 300m;
        // ratio = 300 / 200 = 1.5 (1.0 - 2.0)

        // Act
        var risk = service.AssessRisk(totalBackStake, totalLiability);

        // Assert
        Assert.Equal(RiskLevel.High, risk);
    }

    [Fact]
    public void AssessRisk_CriticalRatio_ReturnsCritical()
    {
        // Arrange
        var service = new RiskAssessmentService();
        decimal totalBackStake = 100m;
        decimal totalLiability = 250m;
        // ratio = 250 / 100 = 2.5 (>= 2.0)

        // Act
        var risk = service.AssessRisk(totalBackStake, totalLiability);

        // Assert
        Assert.Equal(RiskLevel.Critical, risk);
    }

    [Fact]
    public void GetMaximumExposure_ValidInput_Returns120Percent()
    {
        // Arrange
        var service = new RiskAssessmentService();
        decimal totalLiability = 500m;

        // Act
        var maxExposure = service.GetMaximumExposure(totalLiability);

        // Assert
        // maxExposure = 500 * 1.2 = 600
        Assert.Equal(600m, maxExposure);
    }
}
