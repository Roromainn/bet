namespace BetTracker.Core.Services;

/// <summary>
/// Service for sending notifications about arbitrage alerts and activity logging.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Send an arbitrage alert notification when a profitable opportunity is found.
    /// </summary>
    /// <param name="bookmaker1">Name of the first bookmaker</param>
    /// <param name="bookmaker2">Name of the second bookmaker</param>
    /// <param name="profitMargin">The profit margin percentage</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task SendArbitrageAlertAsync(string bookmaker1, string bookmaker2, decimal profitMargin);

    /// <summary>
    /// Log an activity event.
    /// </summary>
    /// <param name="activityType">The type of activity being logged</param>
    /// <param name="details">Details about the activity</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task LogActivityAsync(string activityType, string details);
}
