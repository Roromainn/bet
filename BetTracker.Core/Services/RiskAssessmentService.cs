using BetTracker.Core.Models;

namespace BetTracker.Core.Services;

/// <summary>
/// Service for assessing risk levels of bets based on stake ratios and exposure.
/// </summary>
public class RiskAssessmentService : IRiskAssessmentService
{
    /// <summary>
    /// Assess the risk level of a bet based on the ratio of liability to back stake.
    /// Risk thresholds:
    /// - ratio < 0.5: Low
    /// - 0.5 <= ratio < 1.0: Medium
    /// - 1.0 <= ratio < 2.0: High
    /// - ratio >= 2.0: Critical
    /// </summary>
    public RiskLevel AssessRisk(decimal totalBackStake, decimal totalLiability)
    {
        // Handle zero back stake edge case
        if (totalBackStake == 0)
        {
            return RiskLevel.Critical;
        }

        decimal ratio = totalLiability / totalBackStake;

        if (ratio < 0.5m)
        {
            return RiskLevel.Low;
        }

        if (ratio < 1.0m)
        {
            return RiskLevel.Medium;
        }

        if (ratio < 2.0m)
        {
            return RiskLevel.High;
        }

        return RiskLevel.Critical;
    }

    /// <summary>
    /// Get the maximum safe exposure with a 20% buffer.
    /// Calculated as totalLiability * 1.2.
    /// </summary>
    public decimal GetMaximumExposure(decimal totalLiability)
    {
        return totalLiability * 1.2m;
    }
}
