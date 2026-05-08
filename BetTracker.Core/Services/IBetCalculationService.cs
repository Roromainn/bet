using BetTracker.Core.Models;

namespace BetTracker.Core.Services;

/// <summary>
/// Service for calculating bet-related metrics like EV (Expected Value) and profits.
/// </summary>
public interface IBetCalculationService
{
    /// <summary>
    /// Calculate EV (Expected Value) for lay bets.
    /// Formula: EV = back_stake * (back_odds - 1) - lay_stake * lay_odds
    /// </summary>
    /// <param name="backStake">The stake on the back bet (often at the first bookmaker)</param>
    /// <param name="backOdds">The odds for the back bet</param>
    /// <param name="layStake">The stake on the lay bet (often at the second bookmaker)</param>
    /// <param name="layOdds">The odds for the lay bet</param>
    /// <returns>The expected value</returns>
    decimal CalculateLayBetEV(decimal backStake, decimal backOdds, decimal layStake, decimal layOdds);

    /// <summary>
    /// Calculate profit for matched bets.
    /// Formula: Profit = (stake1 * odds1) + (stake2 * odds2) - total_stakes
    /// </summary>
    /// <param name="stake1">The first stake amount</param>
    /// <param name="odds1">The first odds</param>
    /// <param name="stake2">The second stake amount</param>
    /// <param name="odds2">The second odds</param>
    /// <returns>The calculated profit</returns>
    decimal CalculateMatchedBetProfit(decimal stake1, decimal odds1, decimal stake2, decimal odds2);

    /// <summary>
    /// Calculate actual EV based on match result and real odds.
    /// </summary>
    /// <param name="calcul">The Calcul entity with theoretical values</param>
    /// <param name="execution">The Execution entity with actual values and results</param>
    /// <returns>The actual expected value</returns>
    decimal CalculateActualEV(Calcul calcul, Execution execution);

    /// <summary>
    /// Calculate coverage loss (perte de couverture) for a bet.
    /// </summary>
    /// <param name="stake1">The first stake</param>
    /// <param name="odds1">The first odds</param>
    /// <param name="stake2">The second stake</param>
    /// <param name="odds2">The second odds</param>
    /// <returns>The coverage loss amount</returns>
    decimal CalculateCoverageLoss(decimal stake1, decimal odds1, decimal stake2, decimal odds2);
}
