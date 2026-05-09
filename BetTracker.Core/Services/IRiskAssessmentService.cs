using BetTracker.Core.Models;

namespace BetTracker.Core.Services;

/// <summary>
/// Service for assessing risk levels of bets based on stake ratios and exposure.
/// </summary>
public interface IRiskAssessmentService
{
    /// <summary>
    /// Assess the risk level of a bet based on the ratio of liability to back stake.
    /// </summary>
    /// <param name="totalBackStake">Total amount staked on the back side.</param>
    /// <param name="totalLiability">Total liability exposure on the lay side.</param>
    /// <returns>The assessed risk level.</returns>
    RiskLevel AssessRisk(decimal totalBackStake, decimal totalLiability);

    /// <summary>
    /// Get the maximum safe exposure, calculated as liability with a 20% buffer.
    /// </summary>
    /// <param name="totalLiability">Total liability exposure.</param>
    /// <returns>Maximum safe exposure (liability * 1.2).</returns>
    decimal GetMaximumExposure(decimal totalLiability);
}
