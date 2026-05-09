namespace BetTracker.Core.Models;

/// <summary>
/// Represents the risk level assessment for a bet configuration.
/// </summary>
public enum RiskLevel
{
    /// <summary>
    /// Low risk - liability to back stake ratio is low (< 0.5)
    /// </summary>
    Low,

    /// <summary>
    /// Medium risk - liability to back stake ratio is moderate (0.5 - 1.0)
    /// </summary>
    Medium,

    /// <summary>
    /// High risk - liability to back stake ratio is elevated (1.0 - 2.0)
    /// </summary>
    High,

    /// <summary>
    /// Critical risk - liability to back stake ratio is very high (>= 2.0)
    /// </summary>
    Critical
}
