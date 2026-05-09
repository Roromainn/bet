using BetTracker.Core.Services;

namespace BetTracker.Tests.Services;

/// <summary>
/// Unit tests for NotificationService.
/// </summary>
public class NotificationServiceTests
{
    [Fact]
    public async Task SendArbitrageAlertAsync_ValidInputs_Completes()
    {
        // Arrange
        var service = new NotificationService();
        string bookmaker1 = "Betfair";
        string bookmaker2 = "Pinnacle";
        decimal profitMargin = 2.50m;

        // Act
        await service.SendArbitrageAlertAsync(bookmaker1, bookmaker2, profitMargin);

        // Assert
        // No exception should be thrown; async operation completes successfully
        Assert.True(true);
    }

    [Fact]
    public async Task SendArbitrageAlertAsync_WithNegativeMargin_Completes()
    {
        // Arrange
        var service = new NotificationService();
        string bookmaker1 = "bet365";
        string bookmaker2 = "William Hill";
        decimal profitMargin = -1.50m;

        // Act
        await service.SendArbitrageAlertAsync(bookmaker1, bookmaker2, profitMargin);

        // Assert
        // Edge case: should handle negative margin gracefully
        Assert.True(true);
    }

    [Fact]
    public async Task LogActivityAsync_ValidInputs_Completes()
    {
        // Arrange
        var service = new NotificationService();
        string activityType = "BET_PLACED";
        string details = "Placed €100 at 2.5 odds on Betfair";

        // Act
        await service.LogActivityAsync(activityType, details);

        // Assert
        // No exception should be thrown; async operation completes successfully
        Assert.True(true);
    }

    [Fact]
    public async Task LogActivityAsync_WithEmptyDetails_Completes()
    {
        // Arrange
        var service = new NotificationService();
        string activityType = "SYNC_COMPLETE";
        string details = "";

        // Act
        await service.LogActivityAsync(activityType, details);

        // Assert
        // Edge case: should handle empty details gracefully
        Assert.True(true);
    }
}
