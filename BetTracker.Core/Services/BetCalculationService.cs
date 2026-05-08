using BetTracker.Core.Models;

namespace BetTracker.Core.Services;

/// <summary>
/// Implementation of bet calculation service for EV and profit calculations.
/// </summary>
public class BetCalculationService : IBetCalculationService
{
    /// <summary>
    /// Calculate EV (Expected Value) for lay bets.
    /// Formula: EV = back_stake * (back_odds - 1) - lay_stake * lay_odds
    /// </summary>
    public decimal CalculateLayBetEV(decimal backStake, decimal backOdds, decimal layStake, decimal layOdds)
    {
        if (backOdds <= 0 || layOdds <= 0)
        {
            throw new ArgumentException("Odds must be greater than 0");
        }

        // Potential win from back bet
        var backWin = backStake * (backOdds - 1);

        // Potential loss from lay bet
        var layLoss = layStake * layOdds;

        // EV calculation
        return backWin - layLoss;
    }

    /// <summary>
    /// Calculate profit for matched bets.
    /// When both bets hit, the profit is guaranteed if properly matched.
    /// Formula: Profit = (stake1 * odds1) - stake1 + (stake2 * odds2) - stake2
    /// Simplified: Profit = stake1 * (odds1 - 1) + stake2 * (odds2 - 1) - total_stakes
    /// </summary>
    public decimal CalculateMatchedBetProfit(decimal stake1, decimal odds1, decimal stake2, decimal odds2)
    {
        if (odds1 <= 0 || odds2 <= 0)
        {
            throw new ArgumentException("Odds must be greater than 0");
        }

        if (stake1 < 0 || stake2 < 0)
        {
            throw new ArgumentException("Stakes cannot be negative");
        }

        // In a perfectly matched bet scenario:
        // One bet wins and covers the other
        var totalStakes = stake1 + stake2;
        var returns1 = stake1 * odds1;
        var returns2 = stake2 * odds2;

        // Expected profit if one scenario hits
        // This assumes the back bet wins and the lay bet loses (most conservative)
        var profit = Math.Min(returns1, returns2) - totalStakes;

        return profit;
    }

    /// <summary>
    /// Calculate actual EV based on match result and real odds.
    /// </summary>
    public decimal CalculateActualEV(Calcul calcul, Execution execution)
    {
        if (calcul == null || execution == null)
        {
            throw new ArgumentNullException("Calcul and Execution cannot be null");
        }

        // Get actual stakes and odds from execution
        var actualEV = CalculateLayBetEV(
            execution.MiseReelleBook1,
            execution.CoteReelleBook1,
            execution.MiseReelleBook2,
            execution.CoteReelleBook2
        );

        return actualEV;
    }

    /// <summary>
    /// Calculate coverage loss (perte de couverture) for a bet.
    /// This is the amount lost when the coverage isn't perfect.
    /// </summary>
    public decimal CalculateCoverageLoss(decimal stake1, decimal odds1, decimal stake2, decimal odds2)
    {
        if (odds1 <= 0 || odds2 <= 0)
        {
            throw new ArgumentException("Odds must be greater than 0");
        }

        if (stake1 < 0 || stake2 < 0)
        {
            throw new ArgumentException("Stakes cannot be negative");
        }

        // Coverage loss is the gap between what you get from one outcome
        // and what you lose in the other
        var outcome1 = stake1 * odds1;      // Return if bet 1 wins
        var outcome2 = stake2 * odds2;      // Return if bet 2 wins
        var totalStakes = stake1 + stake2;

        // The loss is the difference between stakes and minimum return
        var minOutcome = Math.Min(outcome1, outcome2);
        var loss = totalStakes - minOutcome;

        return loss > 0 ? loss : 0;
    }
}
