namespace BetTracker.Core.Services;

/// <summary>
/// Implementation of notification service for sending alerts and logging activities.
/// This is a stub implementation that logs to console.
/// </summary>
public class NotificationService : INotificationService
{
    /// <summary>
    /// Send an arbitrage alert notification to console.
    /// Format: "[ARBITRAGE] {bookmaker1} vs {bookmaker2}: +{profitMargin:F2}%"
    /// </summary>
    public Task SendArbitrageAlertAsync(string bookmaker1, string bookmaker2, decimal profitMargin)
    {
        Console.WriteLine($"[ARBITRAGE] {bookmaker1} vs {bookmaker2}: +{profitMargin:F2}%");
        return Task.CompletedTask;
    }

    /// <summary>
    /// Log an activity event to console.
    /// Format: "[{activityType}] {details}"
    /// </summary>
    public Task LogActivityAsync(string activityType, string details)
    {
        Console.WriteLine($"[{activityType}] {details}");
        return Task.CompletedTask;
    }
}
