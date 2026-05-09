using BetTracker.Core.Models;

namespace BetTracker.Core.Services;

/// <summary>
/// Service for finding arbitrage opportunities between bookmakers.
/// </summary>
public interface IArbitrageFinderService
{
    /// <summary>
    /// Find arbitrage opportunities based on a minimum profit threshold.
    /// </summary>
    /// <param name="minProfitThreshold">The minimum profit margin threshold to consider an opportunity</param>
    /// <returns>Collection of arbitrage opportunities that meet the profit threshold</returns>
    Task<IEnumerable<ArbitrageOpportunity>> FindArbitrageOpportunitiesAsync(decimal minProfitThreshold);
}
